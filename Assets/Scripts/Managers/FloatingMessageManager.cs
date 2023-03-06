using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingMessageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;
    [SerializeField]
    private TextMeshProUGUI _floatingText;

    private static FloatingMessageManager _instance;

    public static FloatingMessageManager instance 
    {get {return _instance;}}

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnFloatingText(string message)
    {
        SpawnFloatingText(message, 3.0f);
    }

    public void SpawnFloatingText(string message, float duration=3.0f)
    {
        if(_parent != null && _floatingText != null)
        {
            GameObject gameObject = Instantiate(_floatingText.gameObject, _parent.transform);

            gameObject.GetComponent<TextMeshProUGUI>().text = message;

            Destroy(gameObject, duration);
        }
    }

    public void SpawnFloatingTextControlPosition(string message, Vector3 position, float duration=3.0f)
    {
        if(_parent != null && _floatingText != null)
        {
            GameObject gameObject = Instantiate(_floatingText.gameObject, _parent.transform);
            gameObject.transform.position = Camera.main.WorldToScreenPoint(position);

            gameObject.GetComponent<TextMeshProUGUI>().text = message;

            Destroy(gameObject, duration);
        }
    }
}
