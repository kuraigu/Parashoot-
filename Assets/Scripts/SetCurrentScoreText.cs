using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetCurrentScoreText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentScoreText;

    private void Start()
    {
        if (_currentScoreText != null && ScoreManager.instance != null)
        {
            _currentScoreText.text = ScoreManager.instance.score.ToString();
        }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (_currentScoreText != null && ScoreManager.instance != null)
        {
            _currentScoreText.text = ScoreManager.instance.score.ToString();
        }
    }
}
