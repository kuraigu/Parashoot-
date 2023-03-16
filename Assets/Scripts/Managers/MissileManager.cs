using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private static MissileManager _instance;
    [SerializeField] private GameObject _missilePrefab;
    private List<GameObject> _missilePool = new List<GameObject>();

    [SerializeField]
    private int _poolSize = 10;

    private int _currentIndex = 0;

    public static MissileManager instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

        InitializePool();
    }

    private void InitializePool()
    {
          // create the missile pool
        _missilePool = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject missile = Instantiate(_missilePrefab);
            missile.SetActive(false);
            _missilePool.Add(missile);
        }
    }

    public void SpawnMissile(GameObject target)
    {
        // get a missile from the pool
        GameObject missile = GetMissileFromPool();

        // check if missile and target are not null
        if (missile != null && target != null)
        {
            Vector3 newPos;

            // generate a random value for x-coordinate
            float xPos = UnityEngine.Random.Range(-1f, 2f);
            float yPos = UnityEngine.Random.Range(0.0f, 1.0f);
            // convert the viewport coordinates to world coordinates
            newPos = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, -10f));
            newPos.z = 5f;
            //set the position of the new missile
            missile.transform.position = newPos;
            missile.SetActive(true);

            // get the missile controller component
            MissileController missileController = missile.GetComponent<MissileController>();

            // check if missile controller is not null
            if (missileController != null)
            {
                // set the target for the missile
                missileController.SetTarget(target);
            }
        }
    }

    private GameObject GetMissileFromPool()
    {
        if (_missilePrefab != null)
        {
            if(_currentIndex >  _missilePool.Count) _currentIndex = 0;
            // check if there is an inactive missile in the pool starting from currentIndex
            if (_currentIndex < _missilePool.Count && !_missilePool[_currentIndex].activeInHierarchy)
            {
                GameObject missileTemp = _missilePool[_currentIndex];
                _currentIndex++;
                return missileTemp;
            }

            // if currentIndex is at the end of the list or no inactive missile is found, create a new one
            GameObject missile = Instantiate(_missilePrefab);
            missile.SetActive(false);
            _missilePool.Add(missile);
            _currentIndex = _missilePool.Count - 1;
            return missile;
        }

        return null;
    }

    public void PutMissileBackInPool(GameObject missile)
    {
        missile.SetActive(false);
    }
}