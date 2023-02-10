using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticTextSetter : MonoBehaviour
{
    [SerializeField]
    private TextDataSO _textData;

    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        SetText(_textData);
    }

    private void SetText(TextDataSO textData)
    {
        if(textData != null && _text != null)
        {
            _text.text = _textData.text;
        }
    }
}
