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

        Application.targetFrameRate = 300;

        if(DataManager.instance != null)
        {
            DataManager.instance.LoadData();
        }
    }


    private void Start()
    {
        if(DataManager.instance != null && FloatingMessageManager.instance != null && EnemyManager.instance != null)
        {
            if(DataManager.instance.data.firstTime)
            {
                FloatingMessageManager.instance.SpawnFloatingText("<b><color=#FFE0A6>Swipe based on the direction shown</b>", 60);
                FloatingMessageManager.instance.SpawnFloatingText("<b><color=#FFE0A6>Consecutive misktakes mean GAME OVER</b>", 60);

                DataManager.instance.data.firstTime = false;
                DataManager.instance.SaveData();
            }
        }
    }
}
