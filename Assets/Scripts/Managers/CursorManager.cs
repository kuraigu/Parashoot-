using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;

    private bool _isHoveringUI;

    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;

    public static CursorManager instance
    { get { return _instance; } }

    public bool isHoveringUI
    { get { return _isHoveringUI; } }


    void Awake()
    {
        _instance = this;
    }

    void Update()
    {

        _isHoveringUI = false;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isHoveringUI = true;
        }

        // Check if the user is touching the screen
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                EventSystem.current.IsPointerOverGameObject(touch.fingerId);
            }
        }
    }
}
