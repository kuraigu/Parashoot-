using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private static MissileManager _instance;

    [SerializeField]
    private GameObject _missile;

    public static MissileManager instance
    { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnMissile(GameObject target)
    {
        // check if _missile prefab and target are not null
        if (_missile != null && target != null)
        {
            // instantiate a new missile
            GameObject newMissile = Instantiate(_missile);
            Vector3 newPos;

            // generate a random value for x-coordinate
            float xPos = UnityEngine.Random.Range(-1f, 2f);
            float yPos = UnityEngine.Random.Range(0.0f, 1.0f);
            // convert the viewport coordinates to world coordinates
            newPos = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, -10f));
            //set the position of the new missile
            newMissile.transform.position = newPos;
            // get the missile controller component
            MissileController missileController = newMissile.GetComponent<MissileController>();

            // check if missile controller is not null
            if (missileController != null)
            {
                // set the target for the missile
                missileController.SetTarget(target);
            }
        }
    }
}
