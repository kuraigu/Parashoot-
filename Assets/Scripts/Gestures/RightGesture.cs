using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Right Gesture", menuName = "Gestures/Right")]
public class RightGesture : GestureSO
{
    public override bool CheckSimilarity(List<Vector2> points)
    {
        Vector2 totalDirections = Vector2.zero;

        //points = Recognizer.Utils.NormalizeGesture(points);

        if (points.Count > 3)
        {

            for (int i = 1; i < points.Count; i++)
            {
                totalDirections += points[i] - points[i - 1];
            }

            totalDirections /= (points.Count - 1);

            if (totalDirections.normalized.x >= 0.7f)
            {
                if (totalDirections.normalized.y <= 0.5f && totalDirections.y >= -0.5f)
                {
                    DebugHandler.Log(_gestureName + " detected");
                    DebugHandler.Log("Direction is " + totalDirections.normalized);
                    return true;
                }
            }
        }

        DebugHandler.Log("Direction is " + totalDirections.normalized);
        DebugHandler.Log("Not " + _gestureName);
        return false;
    }
}
