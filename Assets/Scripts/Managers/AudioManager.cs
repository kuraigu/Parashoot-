using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private bool _isMusicAllowed;

    [SerializeField]
    private float _musicVolume;

    [SerializeField]
    private AudioSource _audioSc;

    [SerializeField]
    private Sound _backgroundMusic;

    [SerializeField]
    private bool _isLoop;

    public static AudioManager instance
    { get { return _instance; } }

    public float musicVolume
    { get { return _musicVolume; } }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (_isMusicAllowed)
        {
            StartCoroutine(PlayBackgroundMusic(1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckBackgroundMusicStatus();
    }


    public void CheckBackgroundMusicStatus()
    {
        if (!_isMusicAllowed)
        {
            if (_audioSc != null)
            {
                if (_audioSc.isPlaying)
                {
                    _audioSc.Stop();
                }
            }
        }

        else
        {
            if (_audioSc != null)
            {
                if (!_audioSc.isPlaying)
                {
                    StartCoroutine(PlayBackgroundMusic(3f));
                }
            }
        }
    }


    public IEnumerator PlayBackgroundMusic(float fadeTime)
    {
        if (_audioSc != null && _backgroundMusic != null)
        {
            // Set the audio clip and loop settings
            _audioSc.clip = _backgroundMusic.audioClip;
            _audioSc.loop = true;

            // Play the audio with no volume
            _audioSc.volume = 0f;
            _audioSc.Play();

            // Gradually increase the volume over the fade-in time
            float t = 0f;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                _audioSc.volume = Mathf.Lerp(0f, 1f, t / fadeTime);
                yield return null;
            }

            // Set the volume to the final value
            _audioSc.volume = 1f;
        }
    }


    public void StopBackgroundMusic()
    {
        if (_audioSc != null && _backgroundMusic != null)
        {
            _audioSc.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0f, 1.0f);

        _musicVolume = volume;
        AudioListener.volume = _musicVolume;
    }
}
