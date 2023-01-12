using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognizerManager : MonoBehaviour
{
    private static RecognizerManager _instance;
    [SerializeField]
    private List<GestureSO> _gestureList = new List<GestureSO>();

    public static RecognizerManager instance
    { get { return _instance; } }
    public List<GestureSO> gestureList
    { get { return _gestureList; } }


    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckAllGestures(Vector2 directionNormalized)
    {
        foreach (GestureSO gesture in _gestureList)
        {
            if(gesture.CheckSimilarity(directionNormalized))
            {
                break;
            }
        }
    }
}
