using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSO : ScriptableObject
{
    [SerializeField]
    private string _gestureName;

    [SerializeField]
    Sprite _sprite;

    public string gestureName
    { get { return _gestureName; } }

    public Sprite sprite
    { get { return _sprite; } }

    protected virtual void InvokeEvent()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.OnGestureTriggered?.Invoke(this);
        }
    }

    public virtual bool CheckSimilarity(Vector2 directionNormalized)
    {
        return false;
    }
}
