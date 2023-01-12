using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;

public class EnemyGesturePanel : MonoBehaviour
{
    [SerializeField]
    private Image _gestureImage;
    [SerializeField]
    private TextMeshProUGUI _numOfGesturesText;

    private List<GestureSO> _gestureList = new List<GestureSO>();


    private void Update()
    {
    }

    private void OnDestroy()
    { 
    }


    public void InitializePanel(GestureSO[] gestures)
    {
        _gestureList = gestures.ToList();

        SetImage();
        SetText();
    }


    public void RemoveCurrentGesture()
    {
        if(_gestureList.Count > 0)
        {
            _gestureList.RemoveAt(0);

            SetImage();
            StartCoroutine(BlinkGestureImage());
            SetText();
        }

        else
        {
            this.gameObject.SetActive(false);
        }
    }


    private void SetImage()
    {
        if(_gestureList.Count > 0)
        {
            _gestureImage.sprite = _gestureList.First()?.sprite;
        }
    }

    
    public void SetText()
    {
        if(_numOfGesturesText != null)
        {
            _numOfGesturesText.text = _gestureList.Count.ToString();
        }
    }


    private IEnumerator BlinkGestureImage()
    {
        _gestureImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _gestureImage.gameObject.SetActive(true);
    }
}
