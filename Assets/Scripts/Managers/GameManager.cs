using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// GameManager is a MonoBehaviour class that manages the game's global state and events
/// </summary>
/// <author> Innoh Reloza </author>
public class GameManager : MonoBehaviour
{   
    private static GameManager _instance;
    /// <summary>
    /// The singleton instance of the GameManager, which allows for easy access to the class from other scripts
    /// </summary>
    public static GameManager instance
    { get { return _instance; } }

    // <summary>
    /// UnityEvent that gets triggered when a gesture is recognized. It carries the GestureSO (ScriptableObject) data of the recognized gesture
    /// </summary>
    public UnityAction<GestureSO> OnGestureTriggered;

    /// <summary>
    /// UnityEvent that gets triggered when an enemy is defeated. It can be used for various purposes such as triggering a sound effect or increasing the player's score
    /// </summary>
    public UnityAction OnEnemyDeath;


    public void TogglePause()
    {
        if(Time.timeScale == 0) Time.timeScale = 1;
        else if(Time.timeScale == 1) Time.timeScale = 0;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }

    public void Retry()
    {
        UnPause();

        if(EnemyManager.instance != null) EnemyManager.instance.ClearAllEnemies();
        if(RecognizerManager.instance != null) 
        {
            RecognizerManager.instance.EnableRecognition();
            RecognizerManager.instance.ClearWrongGesturesContainer();
        }
    }
    private void Awake()
    {
        _instance = this;
        // set the game's target frame rate to 300
        Application.targetFrameRate = 300;
        // limit the frame rate to 60 if it's lower than 60
        if (Application.targetFrameRate < 60)
        {
            Application.targetFrameRate = 60;
        }

        if(DataManager.instance != null)
        {
            DataManager.instance.LoadData();
        }
    }
}
