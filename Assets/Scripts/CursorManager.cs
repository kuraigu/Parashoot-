using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;

    private bool _isHoveringUI;

    public static CursorManager instance
    {get {return _instance;}}

    public bool isHoveringUI
    {get {return _isHoveringUI;}}


    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        _isHoveringUI  = false;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isHoveringUI = true;
        }
    }
}
