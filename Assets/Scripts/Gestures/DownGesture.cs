using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Down Gesture", menuName = "Gestures/Down")]
public class DownGesture : GestureSO
{
    public override bool CheckSimilarity(List<Vector2> points)
    {
        Vector2 start = points[0];
        Vector2 end = points[points.Count - 1];
        Vector2 totalDirections = Vector2.zero;
        float totalDistance = 0f;

        for (int i = 1; i < points.Count; i++)
        {
            totalDirections += points[i] - points[i - 1];
            totalDistance += Vector2.Distance(points[i], points[i - 1]);
        }

        totalDirections /= (points.Count - 1);

        if (totalDirections.normalized.y <= -0.7f && totalDistance > 0.2f)
        {
            if (totalDirections.normalized.x <= 0.5f && totalDirections.x >= -0.5f)
            {
                DebugHandler.Log(_gestureName + " detected");
                DebugHandler.Log("Direction is " + totalDirections.normalized);
                return true;
            }
        }

        DebugHandler.Log("Direction is " + totalDirections.normalized);
        DebugHandler.Log("Not " + _gestureName);
        return false;
    }
}
