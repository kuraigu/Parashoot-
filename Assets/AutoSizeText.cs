using TMPro;
using UnityEngine;

public class AutoSizeText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textComponent;
    [SerializeField]
    private RectTransform _rectTransform;

    private void Start()
    {
        if(_textComponent != null && _rectTransform != null)
        {
              float textWidth = _textComponent.preferredWidth;
        float textHeight = _textComponent.preferredHeight;

        _rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        }
      
    }
}