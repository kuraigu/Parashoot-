using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Unity.Collections;

/// <summary>
/// The EnemyManager class is responsible for spawning enemies in a game over a period of time.
/// It provides a way to set the spawn interval, the prefab of the enemy to spawn, and the position where the enemies will be spawned.
/// Additionally, it keeps track of the time since the last enemy was spawned.
/// </summary>
/// <author> Innoh Reloza </author>
public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    [Header("Spawning Parameters")]
    [SerializeField]
    private FreeMatrix.Utils.Range<float> _timeToSpawnEnemy;
    [SerializeField]
    private float _timeChangeDuration;
    [SerializeField]
    private float _timeDecrementAmount;
    [SerializeField]
    private FreeMatrix.Utils.Range<uint> _enemyToSpawnPerCycle;

    [Header("Chance Parameters")]
    [SerializeField]
    private float _totalSpawnChances;

    [Header("Boss Parameters")]
    [SerializeField]
    private float _timeToSpawnBoss;

    [SerializeField]
    private SpawnState _spawnState;

    private List<Enemy> _enemyListReference = new List<Enemy>();
    private List<Enemy> _enemyList = new List<Enemy>();

    private float _lastEnemyXPosition;
    private Enemy _boss;

    private Coroutine _enemyCoroutine;
    private Coroutine _bossCoroutine;

    public enum SpawnState
    {
        None, 
        Enemy, 
        Boss
    }

    private void Awake()
    {
        _instance = this;

        _spawnState = SpawnState.Enemy;

        StartCoroutine(DecrementTimer(_timeChangeDuration));
        _enemyCoroutine = StartCoroutine(RunTimer(_timeToSpawnEnemy.current));
        _bossCoroutine = StartCoroutine(RunBossTimer(_timeToSpawnBoss));

        SortEnemyListByBiggerSpawnChance();
        GetTotalSpawnChance();
    }

    private void Update()
    {
        switch(_spawnState)
        {
            default:
                break;

            case SpawnState.None:
                break;

            case SpawnState.Enemy:

                RemoveNullEnemies();

                break;

            case SpawnState.Boss:

                if(_boss == null)
                {
                    _enemyCoroutine = StartCoroutine(RunTimer(_timeToSpawnEnemy.current));
                    _bossCoroutine = StartCoroutine(RunBossTimer(_timeToSpawnBoss));

                    _spawnState = SpawnState.Enemy;
                }

                break;
        }
    }

    /// <summary>
    /// Calculates the total spawn chance by summing up the individual spawn chances of all enemies.
    /// </summary>
    /// <author>Innoh Reloza</author>
    private void GetTotalSpawnChance()
    {
        foreach(Enemy enemy in _enemyListReference)
        {
            _totalSpawnChances += enemy.spawnChance;
        }
    }


    private void RemoveNullEnemies()
    {
        _enemyList.RemoveAll(e => e == null || e.isDead);
    }


    /// <summary>
    /// Sorts the _enemyList by spawn chance in ascending order, allowing for easy access to the enemies with the highest spawn chance.
    /// </summary>
    /// <author>Innoh Reloza</author>
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


    /// <summary>
    /// This private IEnumerator function, named "RunTimer", is used to run a timer for the specified duration. 
    /// Once the timer has completed, it will spawn an enemy. 
    /// This function should be called to start the timer.
    /// </summary>
    /// <param name="timer">The duration of the timer in seconds</param>
    /// <returns>An enumerator that can be used in a Coroutine</returns>
    /// <author>Innoh Reloza</author>
    private IEnumerator RunTimer(float timer)
    {
        while(true)
        {
            yield return new WaitForSeconds(timer);

            SpawnEnemy();
        }
    }


    /// <summary>
    /// This private IEnumerator function, named "DecrementTimer", is used to decrement the timer of the class by a specified duration over time.
    /// This function is typically used to make the time between enemy spawns decrease over time.
    /// </summary>
    /// <param name="duration">The duration of time in seconds to decrement the timer by</param>
    /// <returns>An enumerator that can be used in a Coroutine</returns>
    private IEnumerator DecrementTimer(float duration)
    {

        while (true)
        {
            Debug.Log("Awaiting Time Decremention");
            yield return new WaitForSeconds(duration);

            if (_timeToSpawnEnemy.current <= _timeToSpawnEnemy.min)
            {
                break;
            }

            else
            {
                _timeToSpawnEnemy.current -= _timeDecrementAmount;
            }
        }
    }


    /// <summary>
    /// Runs a boss timer that waits for a specified amount of time before spawning a boss.
    /// </summary>
    /// <param name="timer">The amount of time to wait before spawning the boss.</param>
    /// <returns></returns>
    private IEnumerator RunBossTimer(float timer)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(timer);

        StopCoroutine(_enemyCoroutine);
        StopCoroutine(_bossCoroutine);

        // Spawn the boss
        SpawnBoss();     
    }


    /// <summary>
    /// Spawns the boss if spawning is allowed
    /// </summary>
    private void SpawnBoss()
    {
        // check if AssetsGameScene instance exists
        if (AssetsGameScene.instance != null)
        {
            // instantiate boss object
            _boss = Instantiate(AssetsGameScene.instance.enemy.bossList[0]);
            Vector3 newPos = GenerateRandomPos();
            newPos.y += 1;
            newPos.x = 0;
            newPos.z = 5;
            _boss.transform.position = newPos;
            _boss.allowGesturesDestroy = true;

            _spawnState = SpawnState.Boss;
        }
    }

    /// <summary>
    /// Gets an enemy based on its spawn chance
    /// </summary>
    /// <returns>Enemy object</returns>
    private Enemy GetBySpawnChance()
    {
        if (_enemyListReference?.Count > 0)
        {
            float chance = UnityEngine.Random.Range((float)0, (float)_totalSpawnChances);

            foreach (Enemy enemy in _enemyListReference)
            {
                if ((chance -= enemy.spawnChance) <= 0) return enemy;
            }
        }

        return null;
    }



    /// <summary>
    /// Spawns an enemy if spawning is allowed
    /// </summary>
    private void SpawnEnemy()
    {
        // Check if AssetsGameScene is not null and if it has a list of enemies
        if (AssetsGameScene.instance != null && AssetsGameScene.instance.enemy.enemyList != null)
        {
            // Randomizes the number of enemy to spawn per spawning cycle
            uint numOfEnemiesToSpawn = (uint)UnityEngine.Random.Range(_enemyToSpawnPerCycle.min, _enemyToSpawnPerCycle.max);

            for(uint i = 0; i < numOfEnemiesToSpawn; i++)
            {
                // Create an instance of the enemy using the GetBySpawnChance method
                Enemy enemy = Instantiate(GetBySpawnChance(), Vector3.zero, Quaternion.identity);

                // Check if the enemy was instantiated successfully
                if (enemy != null)
                {
                    // Add the enemy to the list and set its position
                    _enemyList.Add(enemy);
                    _enemyList.Last().transform.position = GenerateRandomPos();
                    _lastEnemyXPosition = _enemyList.Last().transform.position.x;
                    //_enemyList.Last().gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidBody);
                    //rigidBody.drag = _enemyFallDrag;


                    // Check if this is the first enemy in the list
                    //if (_enemyList.Count == 1)
                    //{
                    // Allow gestures to be destroyed for the first enemy
                    _enemyList.Last().allowGesturesDestroy = true;
                    //}
                }
            }
        }
    }


    /// <summary>
    /// Generates a random position for spawning enemies
    /// </summary>
    /// <returns>Vector3 representing the position</returns>
    private Vector3 GenerateRandomPos()
    {
        float xValue = UnityEngine.Random.Range(0.1f, 0.9f);
        float zValue = UnityEngine.Random.Range(1.0f, 5.0f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(xValue, 1.2f, 10.0f));
        pos.z = zValue;

        if (IsPositionCloseToLast(pos))
        {
            return GenerateRandomPos();
        }

        return pos;
    }


    /// <summary>
    /// Checks if a given position is close to the last enemy's position
    /// </summary>
    /// <param name="newPos">Vector3 representing the position to check</param>
    /// <returns>True if the position is close, False otherwise</returns>
    private bool IsPositionCloseToLast(Vector3 newPos)
        {
            if (Mathf.Abs(_lastEnemyXPosition - newPos.x) < 2f)
            {
                return true;
            }

            return false;
        }
}
