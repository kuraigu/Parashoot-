using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPrivacyPolicyText : MonoBehaviour
{
    [SerializeField]
    private GameObject _privacyPanel;

    [SerializeField]
    private TextMeshProUGUI _textMesh;

    [SerializeField]
    TextDataSO _textData;

    void Start()
    {
        if(DataManager.instance != null)
        {
            if(DataManager.instance.data.firstTime)
            {
                ShowPrivacyPanel();
            }
        }
    }

    public void ShowPrivacyPanel()
    {
        if(_privacyPanel != null)
        {
            _privacyPanel.SetActive(true);
             _textMesh.text = _textData.text;
        }
    }
}
