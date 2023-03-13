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

    [System.Obsolete]
    [SerializeField]
    private float _spacing = 30.0f;

    private static FloatingMessageManager _instance;

    public static FloatingMessageManager instance
    { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnFloatingText(string message)
    {
        SpawnFloatingText(message, 3.0f);
    }

    public void SpawnFloatingText(string message, float duration = 3.0f)
    {
        if (_parent != null && _floatingText != null)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(_parent.transform, false);
            RectTransform rect =  gameObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 30);

            GameObject textGameObject = Instantiate(_floatingText.gameObject);
            textGameObject.transform.SetParent(gameObject.transform, false);

            //textGameObject.transform.position = gameObject.transform.position;

            textGameObject.GetComponent<TextMeshProUGUI>().text = message;

            if(duration > 3.0f)
            {
                Animator animator = textGameObject.GetComponent<Animator>();

                animator.enabled = false;
            }

            Destroy(gameObject, duration);
        }
    }
}
