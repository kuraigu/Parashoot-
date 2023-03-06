using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using System;

public class RecognizerManager : MonoBehaviour
{
    private static RecognizerManager _instance;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _warningText;
    [SerializeField]
    private GameObject _wrongGesturesContainer;

    [SerializeField]
    private List<GestureSO> _gestureList = new List<GestureSO>();

    [SerializeField]
    private uint _nothingDetected = 0;

    [SerializeField]
    private float _disableRecognitionDuration;

    [SerializeField]
    private int _incorrectGesturesThreshold;

    private bool _allowJamming = false;

    private bool _hasGestureDetected = false;

    private bool _allowGestures;

    private Coroutine _jamCoroutine;

    public static RecognizerManager instance
    { get { return _instance; } }
    public List<GestureSO> gestureList
    { get { return _gestureList; } }

    public uint nothingDetected
    { get { return _nothingDetected; } set { _nothingDetected = value; } }

    public bool allowJamming
    { get { return _allowJamming; } }

    public bool hasGestureDetected
    { get { return _hasGestureDetected; } set { _hasGestureDetected = value; } }

    void Awake()
    {
        _instance = this;
        _allowGestures = true;

        if (_warningText != null)
        {
            _warningText = Instantiate(_warningText);

            if (AssetsGameScene.instance != null)
            {
                _warningText.transform.SetParent(AssetsGameScene.instance.ui.canvas.transform, false);
            }

            _warningText.gameObject.SetActive(false);
        }

        ClearWrongGesturesContainer();
    }

    public void Reset()
    {

    }

    public void CheckAllGestures(List<Vector2> points)
    {
        if (_allowGestures && points.Count > 2)
        {
            foreach (GestureSO g in _gestureList)
            {
                if (g.CheckSimilarity(points))
                {
                    g.InvokeEvent();
                    break;
                }
            }

            if (allowJamming && !_hasGestureDetected)
            {
                _nothingDetected++;

                if (FloatingMessageManager.instance != null)
                {
                    FloatingMessageManager.instance.SpawnFloatingText("<b><color=#ff4242>No Matching Gestures!</color></b>", 3.0f);
                }
            }

            _nothingDetected = Math.Clamp(_nothingDetected, 0, (uint)_incorrectGesturesThreshold);

            if (_nothingDetected > 0 && _allowJamming)
            {
                ShowWrongGesture();
            }

            else if (_nothingDetected <= 0)
            {
                ClearWrongGesturesContainer();
            }

            _hasGestureDetected = false;

            if (_nothingDetected >= _incorrectGesturesThreshold)
            {
                if (EnemyManager.instance.gameOverUI != null && GameManager.instance != null)
                {
                    GameManager.instance.Pause();
                    EnemyManager.instance.gameOverUI.SetActive(true);
                }
            }

            //if (_nothingDetected >= _incorrectGesturesThreshold && _allowJamming)
            //{
            //    DisableRecognition();
            //    _jamCoroutine = StartCoroutine(AutomaticReenableTimer(_disableRecognitionDuration));
            //}

            //_allowJamming = false;

            // Reset
        }
    }

    public bool AllowJamming()
    {
        return _allowJamming = true;
    }

    public bool DisallowJamming()
    {
        return _allowJamming = false;
    }

    public void DisableRecognition()
    {
        _allowGestures = false;
        _nothingDetected = 0;
    }


    public void EnableRecognition()
    {
        _allowGestures = true;
        _nothingDetected = 0;
    }


    private IEnumerator AutomaticReenableTimer(float timer)
    {
        float currentTimer = timer;
        float interval = 0.01f;
        string newText = _warningText.text;
        string nonFormattedtext = _warningText.text;
        _warningText.gameObject.SetActive(true);

        while (currentTimer > 0)
        {
            if (_allowGestures && _nothingDetected <= 0)
            {
                yield return null;
                currentTimer = 0;
                break;
            }
            else
            {

                string formattedText = String.Format(newText, currentTimer.ToString("00.00"));
                _warningText.SetText(formattedText);
                currentTimer -= interval;
                yield return new WaitForSeconds(interval);
            }
        }

        ClearWrongGesturesContainer();
        _warningText.text = nonFormattedtext;
        _warningText.gameObject.SetActive(false);
        EnableRecognition();
    }


    public void ClearWrongGesturesContainer()
    {
        if (_wrongGesturesContainer != null)
        {
            for (int i = 0; i < _wrongGesturesContainer.transform.childCount; i++)
            {
                if (_wrongGesturesContainer.transform.GetChild(i) != null)
                    _wrongGesturesContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    private void ShowWrongGesture()
    {
        if (_wrongGesturesContainer != null)
        {
            if (_nothingDetected <= _wrongGesturesContainer.transform.childCount && _nothingDetected >= 1)
            {
                GameObject tempGameObject = _wrongGesturesContainer.transform.GetChild((int)_nothingDetected - 1).gameObject;

                if (tempGameObject != null)
                {
                    tempGameObject.SetActive(true);
                }
            }
        }
    }
}
