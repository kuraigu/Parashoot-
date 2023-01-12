using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LineDrawer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private List<Vector2> _pointList = new List<Vector2>();
    [SerializeField]
    private float _accuracy;

    public List<Vector2> pointList
    { get { return _pointList; } }
    public void UpdateLine(Vector2 position)
    {
        if(IsLineRendererNotNull())
        {
            if(_pointList.Count == 0)
            {
                SetPoint(position);
                Debug.Log("No points exist, creating new one");

                return;
            }

            else
            {
                if(Vector2.Distance(position, _pointList.Last()) > _accuracy)
                {
                    SetPoint(position);
                }
            }
        }
    }


    private bool IsLineRendererNotNull()
    {
        if (_lineRenderer != null) return true;

        return false;
    }


    private void SetPoint(Vector2 point)
    {
        if(IsLineRendererNotNull())
        {
            _pointList.Add(point);

            _lineRenderer.positionCount = _pointList.Count;
            _lineRenderer.SetPosition(_pointList.Count - 1, point);

            Debug.Log("Setting new point!");
        }
    }
}