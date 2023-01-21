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
    private List<GestureSO> _gestureList = new List<GestureSO>();

    [SerializeField]
    private uint _nothingDetected = 0;

    [SerializeField]
    private float _disableRecognitionDuration;

    private bool _allowGestures;

    private Coroutine _jamCoroutine;

    public static RecognizerManager instance
    { get { return _instance; } }
    public List<GestureSO> gestureList
    { get { return _gestureList; } }

    public uint nothingDetected
    { get { return _nothingDetected; } set { _nothingDetected = value; } }

    void Awake()
    {
        _instance = this;
        _allowGestures = true;

        _warningText = Instantiate(_warningText);

        if(AssetsGameScene.instance != null)
        {
            _warningText.transform.SetParent(AssetsGameScene.instance.ui.canvas.transform, false);
        }

        _warningText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckAllGestures(List<Vector2> points)
    {
        if (_allowGestures && points.Count > 2)
        {
            _nothingDetected++;
            foreach (GestureSO g in _gestureList)
            {
                if (g.CheckSimilarity(points))
                {
                    g.InvokeEvent();
                    break;
                }
            }

            if (_nothingDetected >= 3)
            {
                DisableRecognition();
                _jamCoroutine = StartCoroutine(AutomaticReenableTimer(_disableRecognitionDuration));
                Debug.Log("NOTHING DETECTED FOR THREE TIMES!");
            }
        }
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

        while (true)
        {
            
            yield return new WaitForSeconds(interval);

            
            string formattedText = String.Format(newText, currentTimer.ToString("00.00"));
            _warningText.SetText(formattedText);
            currentTimer -= interval;

            if (currentTimer <= 0)
            {
                _warningText.text = nonFormattedtext;
                _warningText.gameObject.SetActive(false);
                EnableRecognition();
                break;
            }
        }
    }
}
