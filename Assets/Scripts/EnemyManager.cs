using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    [SerializeField]
    private float _timeToSpawn;
    [SerializeField]
    private float _minTimeToSpawn;
    [SerializeField]
    private float _timeChangeDuration;
    [SerializeField]
    private float _timeDecrementAmount;
    private bool _isTimerRunning;

    private List<Enemy> _enemyListReference = new List<Enemy>();
    private List<Enemy> _enemyList = new List<Enemy>();

    private Enemy _boss;

    private float _lastEnemyXPosition;

    [SerializeField]
    private float _totalSpawnChances;

    [SerializeField]
    private float _timeToSpawnBoss;

    private bool _allowBossSpawn;
    private bool _allowEnemySpawn;


    private bool _isBossTimerRunning;

    private void Awake()
    {
        _instance = this;

        _isTimerRunning = false;
        _isBossTimerRunning = false;

        _allowBossSpawn = false;
        _allowEnemySpawn = true;

        StartCoroutine(DecrementTimer(_timeChangeDuration));

        SortEnemyListByBiggerSpawnChance();
        GetTotalSpawnChance();
    }

    private void Update()
    {
        if(!_isTimerRunning)
        {
            StartCoroutine(RunTimer(_timeToSpawn));
        }

        if(!_isBossTimerRunning)
        {
            StartCoroutine(RunBossTimer(_timeToSpawnBoss));
        }


        if(_allowBossSpawn && _boss == null)
        {
            _allowEnemySpawn = true;
            _allowBossSpawn = false;

            _isBossTimerRunning = false;
        }
    }


    private void GetTotalSpawnChance()
    {
        foreach(Enemy enemy in _enemyListReference)
        {
            _totalSpawnChances += enemy.spawnChance;
        }
    }

    private void SortEnemyListByBiggerSpawnChance()
    {
        if (AssetsGameScene.instance != null)
        {
            _enemyListReference = AssetsGameScene.instance.enemy.enemyList;

            if (_enemyListReference.Count > 1)
            {
                for (int x = 0; x < _enemyListReference.Count - 1; x++)
                {
                    for (int y = 1; y < _enemyListReference.Count; y++)
                    {
                        if (_enemyListReference[x].spawnChance > _enemyListReference[y].spawnChance)
                        {
                            Enemy temp = _enemyListReference[x];
                            _enemyListReference[x] = _enemyListReference[y];
                            _enemyListReference[y] = temp;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator RunTimer(float timer)
    {
        _isTimerRunning = true;
        yield return new WaitForSeconds(timer);

        _isTimerRunning = false;
        SpawnEnemy();
    }


    private IEnumerator DecrementTimer(float duration)
    {
        Debug.Log("Awaiting Time Decremention");
        yield return new WaitForSeconds(duration);

        if(_timeToSpawn > _minTimeToSpawn)
        {
            Debug.Log("DECREMENTED TIME!");
            _timeToSpawn -= _timeDecrementAmount;
            StartCoroutine(DecrementTimer(duration));
        }
    }

    private IEnumerator RunBossTimer(float timer)
    {
        _isBossTimerRunning = true;
        yield return new WaitForSeconds(timer);

        _allowEnemySpawn = false;
        _allowBossSpawn = true;
        SpawnBoss();
    }


    private void SpawnBoss()
    {
        if(_allowBossSpawn)
        {
            if(AssetsGameScene.instance != null)
            {
                _boss = Instantiate(AssetsGameScene.instance.enemy.bossList[0]);
                Vector3 newPos = GenerateRandomPos();
                newPos.x = 0;
                newPos.z = 5;
                _boss.transform.position = newPos;
            }
        }
    }

    private Enemy GetBySpawnChance()
    {
        if(_enemyListReference?.Count > 0)
        {
            float chance = UnityEngine.Random.Range((float)0, (float)_totalSpawnChances);

            foreach(Enemy enemy in _enemyListReference)
            {
                if ((chance -= enemy.spawnChance) <= 0) return enemy;
            }
        }

        return null;
    }

    private void SpawnEnemy()
    {
        if(_allowEnemySpawn)
        {
            if (AssetsGameScene.instance != null)
            {
                if (AssetsGameScene.instance.enemy.enemyList != null)
                {
                    Enemy enemy = Instantiate(GetBySpawnChance());

                    if (enemy != null)
                    {
                        _enemyList.Add(enemy);

                        _enemyList.Last().transform.position = GenerateRandomPos();

                        _lastEnemyXPosition = _enemyList.Last().transform.position.x;
                    }
                }
            }
        }
    }


    private Vector3 GenerateRandomPos()
    {
        float xValue1 = UnityEngine.Random.Range(0.1f, 0.9f);
        float xValue2 = UnityEngine.Random.Range(0.1f, 0.9f);
        float xValueAve = (xValue1 + xValue2) / 2;
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(xValueAve, 1.2f, 10.0f));
        float ZValue = UnityEngine.Random.Range(1.0f, 5.0f);
        pos.z = ZValue;

        if(IsPositionCloseToLast(pos))
        {
            return GenerateRandomPos();
        }

        return pos;
    }


    private bool IsPositionCloseToLast(Vector3 newPos)
    {
        if (Mathf.Abs(_lastEnemyXPosition - newPos.x) < 2f)
        {
            return true;
        }

        return false;
    }
}
