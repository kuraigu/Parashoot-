using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Obsolete]
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
    }
}
