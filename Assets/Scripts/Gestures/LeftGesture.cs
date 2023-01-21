using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Left Gesture", menuName = "Gestures/Left")]
public class LeftGesture : GestureSO
{
    public override bool CheckSimilarity(List<Vector2> points)
    {
        Vector2 totalDirections = Vector2.zero;

        points = Recognizer.Utils.NormalizeGesture(points);

        if (points.Count > 3)
        {
            for (int i = 1; i < points.Count; i++)
            {
                totalDirections += points[i] - points[i - 1];
            }

            totalDirections /= (points.Count - 1);

            if (totalDirections.normalized.x <= -0.7f)
            {
                if (totalDirections.normalized.y <= 0.5f && totalDirections.y >= -0.5f)
                {
                    Debug.Log(_gestureName + " detected");
                    Debug.Log("Direction is " + totalDirections.normalized);
                    return true;
                }
            }
        }

        Debug.Log("Direction is " + totalDirections.normalized);
        Debug.Log("Not " + _gestureName);
        return false;
    }
}