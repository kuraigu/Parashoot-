using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// EnemyGesturePanel is a MonoBehaviour class that manages the UI for displaying the enemy's current gesture
/// </summary>
/// <author> Innoh Reloza </author>
public class EnemyGesturePanel : MonoBehaviour
{
    [SerializeField] private Image _gestureImage; // Image component for displaying the gesture's sprite
    [SerializeField] private TextMeshProUGUI _numOfGesturesText; // Text component for displaying the number of remaining gestures

    private List<GestureSO> _gestureList = new List<GestureSO>(); // list of gestures for the enemy

    /// <summary>
    /// Initializes the panel with the provided gestures
    /// </summary>
    /// <param name="gestures">Array of GestureSO objects to be displayed on the panel</param>
    public void InitializePanel(GestureSO[] gestures)
    {
        _gestureList = new List<GestureSO>(gestures);
        SetImage();
        SetText();
    }

    /// <summary>
    /// Removes the current gesture from the list and updates the UI
    /// </summary>
    public void RemoveCurrentGesture()
    {
        if (_gestureList.Count > 0)
        {
            _gestureList.RemoveAt(0);
            SetImage();
            StartCoroutine(BlinkGestureImage());
            SetText();
        }

        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the sprite of the gesture image to the current gesture in the list
    /// </summary>
    private void SetImage()
    {
        if (_gestureList.Count > 0)
        {
            _gestureImage.sprite = _gestureList[0]?.sprite;
        }
    }

    /// <summary>
    /// Sets the text of the number of gestures remaining
    /// </summary>
    private void SetText()
    {
        _numOfGesturesText.text = _gestureList.Count.ToString();
    }

    /// <summary>
    /// Blinks the gesture image
    /// </summary>
    /// <returns>IEnumerator for the coroutine</returns>
    private IEnumerator BlinkGestureImage()
    {
        _gestureImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        _gestureImage.gameObject.SetActive(true);
    }
}