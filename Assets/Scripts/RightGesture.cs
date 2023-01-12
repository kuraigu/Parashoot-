using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Right Gesture", menuName = "Gestures/Right")]
public class RightGesture : GestureSO
{
    public override bool CheckSimilarity(Vector2 directionNormalized)
    {
        if (directionNormalized.x > 0.8f)
        {
            InvokeEvent();
            Debug.Log("Gesture: Right!");
            return true;
        }

        return false;
    }
}
