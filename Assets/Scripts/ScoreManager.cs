using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    private uint _score = 0;
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private static ScoreManager _instance;

    public static ScoreManager instance
    { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    public void AddScore(uint score)
    {
        _score += score;
       
        if(_scoreText != null)
        {
            _scoreText.text = _score.ToString();
        }
    }
}
