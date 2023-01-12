using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Up Gesture", menuName="Gestures/Up")]
public class UpGesture : GestureSO
{
    public override bool CheckSimilarity(Vector2 directionNormalized)
    {
        if (directionNormalized.y > 0.8f)
        {
            InvokeEvent();
            Debug.Log("Gesture: UP!");
            return true;
        }

        return false;
    }
}
