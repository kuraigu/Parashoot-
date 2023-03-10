using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectOnAwake : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Sound _onEnableSound;


    void Awake()
    {
        if(_audioSource != null && _onEnableSound != null)
        {
            _audioSource.clip = _onEnableSound.audioClip;
            _audioSource.Play();
        }
    }
}
