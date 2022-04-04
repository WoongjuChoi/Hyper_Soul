using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScopeDemoController : MonoBehaviour
{


    private AudioSource _clickSoundAudioSource;

    void Start()
    {
        Screen.SetResolution(960, 600, false);
        _clickSoundAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //play audio on click if audio source attached
        if (Input.GetMouseButtonDown(0))
        {
            if (_clickSoundAudioSource != null)
            {
                _clickSoundAudioSource.Play();
            }
        }
    }

}

