using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;


public class Recognizer
{
    // Utility functions
    public static class Utils
    {
        public static List<Vector2> NormalizeGesture(List<Vector2> gesturePoints)
        {
            // Find the bounding box of the gesture
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            // Use Parallel.For to parallelize the loop
            Parallel.For(0, gesturePoints.Count, i =>
            {
                if (gesturePoints[i].x < minX) minX = gesturePoints[i].x;
                if (gesturePoints[i].x > maxX) maxX = gesturePoints[i].x;
                if (gesturePoints[i].y < minY) minY = gesturePoints[i].y;
                if (gesturePoints[i].y > maxY) maxY = gesturePoints[i].y;
            });

            // Calculate the center and scale of the gesture
            Vector2 center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
            float scale = Mathf.Max(maxX - minX, maxY - minY);

            // Normalize the gesture points
            List<Vector2> normalizedPoints = new List<Vector2>();
            for (int i = 0; i < gesturePoints.Count; i++)
            {
                Vector2 normalizedPoint = new Vector2(
                    (gesturePoints[i].x - center.x) / scale,
                    (gesturePoints[i].y - center.y) / scale
                );
                normalizedPoints.Add(normalizedPoint);
            }

            return normalizedPoints;
        }
    }
}