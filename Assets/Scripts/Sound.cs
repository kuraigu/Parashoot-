using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    [Range(0,1)]
    private float _volume;
    [SerializeField]
    private float _pitch;

    public AudioClip audioClip
    {get {return _audioClip;}}

    public float volume
    {get {return volume;}}

    public float pitch
    {get {return _pitch;}}
}
