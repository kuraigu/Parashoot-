using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(DataManager.instance.data.settings != null)
        {
            if(_musicSlider != null)
            {
                _musicSlider.value = DataManager.instance.data.settings.musicVolume;
            }
        }
    }

    public void ApplySettings()
    {
        if(DataManager.instance.data.settings != null)
        {
            if(_musicSlider != null)
            {
                DataManager.instance.data.settings.musicVolume = _musicSlider.value;
                DataManager.instance.data.settings.ApplySettings();
                DataManager.instance.SaveData();
            }
        }
    }
}
