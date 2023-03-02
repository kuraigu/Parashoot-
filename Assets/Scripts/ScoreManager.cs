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

    public uint score
    { get { return _score; } }

    private void Awake()
    {
        _instance = this;
    }

    public void AddScore(uint score)
    {
        _score += score;

        if (DataManager.instance != null && _score > DataManager.instance.data.highScore)
        {
            DataManager.instance.data.highScore = _score;
            DataManager.instance.SaveData();
        }

        if (DataManager.instance != null)
        {
            DataManager.instance.data.totalScore += score;
            DataManager.instance.SaveData();
        }

        if (_scoreText != null)
        {
            _scoreText.text = _score.ToString();
        }
    }
}
