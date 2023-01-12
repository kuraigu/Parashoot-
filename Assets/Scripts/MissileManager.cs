using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private static MissileManager _instance;

    [SerializeField]
    private GameObject _missile;

    public static MissileManager instance
    { get { return _instance;} }

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnMissile(GameObject target)
    {
        if(_missile != null && target != null)
        {
            GameObject newMissile = Instantiate(_missile);
            Vector3 newPos;

            int xValue = UnityEngine.Random.Range(1, 3);

            float xPos;
            float yPos = UnityEngine.Random.Range(0.0f, 1.0f);
            if (xValue == 1) xPos = -1;
            else xPos = 2;

            newPos = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, -10f));
            float ZValue = UnityEngine.Random.Range(1.0f, 5.0f);
            newPos.z = ZValue;
            newMissile.transform.position = newPos;

            MissileController missileController = newMissile.GetComponent<MissileController>();

            if(missileController != null)
            {
                missileController.SetTarget(target);
            }
        }
    }
}
