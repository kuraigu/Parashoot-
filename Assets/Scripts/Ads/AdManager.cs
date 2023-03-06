using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private static AdManager _instance;

    [SerializeField]
    private bool _isTesting;

    public static AdManager instance 
    {get {return _instance;}}
    public bool isTesting  
    {get {return _isTesting;}}

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
}
