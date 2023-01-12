using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    { get { return _instance; } }

    public UnityAction<GestureSO> OnGestureTriggered;

    public UnityAction OnEnemyDeath;

    private void Awake()
    {
        _instance = this;

        Application.targetFrameRate = 300;

        if(Application.targetFrameRate < 60)
        {
            Application.targetFrameRate = 60;
        }
    }
}
