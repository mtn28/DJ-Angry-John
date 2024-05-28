using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SettingsMenuManager : MonoBehaviour
{

    public Slider  musicaVol;
    public AudioMixer mainAudioMixer;

    public void ChangeMusicaVolume ()
    {
        mainAudioMixer.SetFloat("Musica", musicaVol.value);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
