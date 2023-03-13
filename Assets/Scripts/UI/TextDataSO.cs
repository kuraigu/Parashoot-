using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextData", fileName = "New TextData")]
public class TextDataSO : ScriptableObject
{
    [SerializeField]
    [TextArea(1, 10000)]
    private string _text;

    public string text 
    {get {return _text;}}
}
