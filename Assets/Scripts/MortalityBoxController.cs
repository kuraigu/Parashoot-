using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalityBoxController : MonoBehaviour
{
    private Vector3 _newPos;

    private void Start()
    {
        _newPos = Camera.main.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(this.transform.position)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position != _newPos)
        {
            this.transform.position = _newPos;
        }
    }
}
