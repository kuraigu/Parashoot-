using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Down Gesture", menuName = "Gestures/Down")]
public class DownGesture : GestureSO
{
    public override bool CheckSimilarity(Vector2 directionNormalized)
    {
        if (directionNormalized.y < -0.8f)
        {
            InvokeEvent();
            Debug.Log("Gesture: DOWN!");
            return true;
        }

        return false;
    }
}
