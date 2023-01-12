using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Vector3 _initialPosition;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _distance;

    // Start is called before the first frame update
    void Start()
    {
        _speed = UnityEngine.Random.Range(-_speed, _speed);
        _initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;
       

        if(Vector3.Distance(_initialPosition, this.transform.position) >= _distance)
        {
            _initialPosition = this.transform.position;

            _speed *= -1;
        }
    }
}
