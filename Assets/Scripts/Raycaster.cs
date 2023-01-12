using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster
{
    public static RaycastHit MouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit);
       
        return hit;
    }
}
