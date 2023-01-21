using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Diagnostics;

public class GestureSO : ScriptableObject
{
    [SerializeField]
    protected string _gestureName;

    [SerializeField]
    protected Sprite _sprite;

    [SerializeField]
    protected List<Vector2> _gesturePoints = new List<Vector2>();

    public string gestureName
    { get { return _gestureName; } }

    public Sprite sprite
    { get { return _sprite; } }

    public virtual bool CheckSimilarity(List<Vector2> points)
    {
        return false;
    }

    public virtual void InvokeEvent()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered?.Invoke(this);
        }
    }

    public virtual void GeneratePoints()
    {

    }
}
