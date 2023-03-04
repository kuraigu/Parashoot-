using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextData", fileName = "New TextData")]
public class TextDataSO : ScriptableObject
{
    [SerializeField]
    [TextArea(50, 0)]
    private string _text;

    public string text 
    {get {return _text;}}
}
