using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityStandardAssets.Cameras;

public class SettingsMenuManager : MonoBehaviour
{

    public Slider musicaVol;
    public AudioMixer mainAudioMixer;
    public Slider sensibilidadeMouseSlider;
    public FreeLookCam freeLookCam;

    void Start()
    {
        // Initialize the sliders with current values
        float currentMusicVolume;
        if (mainAudioMixer.GetFloat("Musica", out currentMusicVolume))
        {
            musicaVol.value = currentMusicVolume;
        }

        sensibilidadeMouseSlider.minValue = 0.1f; // Valor mínimo da sensibilidade
        sensibilidadeMouseSlider.maxValue = 10f;  // Valor máximo da sensibilidade
        sensibilidadeMouseSlider.value = freeLookCam.m_TurnSpeed;

        // Add listeners to the sliders
        musicaVol.onValueChanged.AddListener(delegate { ChangeMusicaVolume(); });
        sensibilidadeMouseSlider.onValueChanged.AddListener(delegate { ChangeMouseSensitivity(); });
    }

    public void ChangeMusicaVolume()
    {
        mainAudioMixer.SetFloat("Musica", musicaVol.value);
    }

    public void ChangeMouseSensitivity()
    {
        freeLookCam.m_TurnSpeed = sensibilidadeMouseSlider.value;
    }
}
