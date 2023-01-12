using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Left Gesture", menuName = "Gestures/Left")]
public class LeftGesture : GestureSO
{
    public override bool CheckSimilarity(Vector2 directionNormalized)
    {
        if (directionNormalized.x < 0.8f)
        {
            InvokeEvent();
            Debug.Log("Gesture: LEFT!");
            return true;
        }

        return false;
    }
}
