using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTotalAcesText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    // Update is called once per frame
    void Update()
    {
        if(DataManager.instance != null)
        {
            if (_text != null)
            {
                _text.text = string.Format("{0:D11}", DataManager.instance.data.totalScore);
            }
        }
    }
}
