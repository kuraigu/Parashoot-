using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetHighScoreText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _highScoreText;

    void Start()
    {
        if (_highScoreText != null && DataManager.instance != null)
        {
            _highScoreText.text = DataManager.instance.data.highScore.ToString();
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (_highScoreText != null && DataManager.instance != null)
        {
            _highScoreText.text = DataManager.instance.data.highScore.ToString();
        }
    }
}
