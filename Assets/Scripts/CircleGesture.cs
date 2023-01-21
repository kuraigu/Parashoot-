using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Circle Gesture", menuName = "Gestures/Circle")]
public class CircleGesture : GestureSO
{
    public override void GeneratePoints()
    {
        _gesturePoints = new List<Vector2>();
        _gesturePoints.Add(new Vector2(50, 50));
        _gesturePoints.Add(new Vector2(52, 49));
        _gesturePoints.Add(new Vector2(53, 47));
        _gesturePoints.Add(new Vector2(54, 45));
        _gesturePoints.Add(new Vector2(54, 42));
        _gesturePoints.Add(new Vector2(54, 39));
        _gesturePoints.Add(new Vector2(53, 37));
        _gesturePoints.Add(new Vector2(52, 35));
        _gesturePoints.Add(new Vector2(50, 34));
        _gesturePoints.Add(new Vector2(48, 35));
        _gesturePoints.Add(new Vector2(47, 37));
        _gesturePoints.Add(new Vector2(46, 39));
        _gesturePoints.Add(new Vector2(46, 42));
        _gesturePoints.Add(new Vector2(46, 45));
        _gesturePoints.Add(new Vector2(47, 47));
        _gesturePoints.Add(new Vector2(48, 49));
        _gesturePoints.Add(new Vector2(50, 50));

        Debug.Log("Done Generating");
    }
}
