using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Click Gesture", menuName = "Gestures/Click")]
public class ClickGesture : GestureSO
{
    public override bool CheckSimilarity(List<Vector2> points)
    {
        points = Recognizer.Utils.NormalizeGesture(points);

        if (points.Count < 3)
        {
            Debug.Log("Click Gesture");
            return true;
        }

        DebugHandler.Log("Not " + _gestureName);
        return false;
    }
}
