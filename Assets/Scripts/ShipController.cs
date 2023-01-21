using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ShipController is a MonoBehaviour class that controls the movement and direction of a ship
/// </summary>
/// <author>Innoh Reloza</author>
public class ShipController : MonoBehaviour
{
    private Vector3 _initialPosition; // the initial position of the ship

    [SerializeField] private float _speed; // the speed of the ship
    [SerializeField] private float _distance; // the distance the ship can travel before reversing direction

    private void Start()
    {
        _speed = UnityEngine.Random.Range(-_speed, _speed); // randomize the speed
        _initialPosition = this.transform.position;
    }

    private void Update()
    {
        this.transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;

        if (Vector3.Distance(_initialPosition, this.transform.position) >= _distance)
        {
            _initialPosition = this.transform.position;
            _speed *= -1;
        }

        // make the ship look in the direction of its movement
        Vector3 lookPos = this.transform.position;
        lookPos.x += _speed * 10;
        this.transform.LookAt(lookPos);
    }
}