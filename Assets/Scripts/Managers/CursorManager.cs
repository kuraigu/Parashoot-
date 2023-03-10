using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;

    private bool _isHoveringUI;

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
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            _isHoveringUI = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _isHoveringUI = true;
            }
        }

        else
        {
            _isHoveringUI = false;

            // Check if the user is touching the screen
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            _isHoveringUI = true;
                            break; // exit the loop once a UI element is found
                        }
                    }
                }
            }
        }
    }
}
