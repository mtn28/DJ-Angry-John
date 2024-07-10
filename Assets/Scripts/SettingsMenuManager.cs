using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;

public class SettingsMenuManager : MonoBehaviour
{
    public Slider musicaVol;
    public AudioMixer mainAudioMixer;
    public Slider sensibilidadeMouseSlider;
    public CinemachineFreeLook cinemachineFreeLook;

    private float defaultXSensitivity = 150f; // Sensibilidade padrão do eixo X
    private float minXSensitivity = 40f; // Sensibilidade mínima do eixo X
    private float maxXSensitivity = 400f; // Sensibilidade máxima do eixo X

    void Start()
    {
        // Initialize the sliders with current values
        float currentMusicVolume;
        if (mainAudioMixer.GetFloat("Musica", out currentMusicVolume))
        {
            musicaVol.value = currentMusicVolume;
        }

        sensibilidadeMouseSlider.minValue = minXSensitivity; // Valor mínimo da sensibilidade
        sensibilidadeMouseSlider.maxValue = maxXSensitivity; // Valor máximo da sensibilidade
        sensibilidadeMouseSlider.value = defaultXSensitivity; // Valor do meio do slider para sensibilidade padrão

        // Add listeners to the sliders
        musicaVol.onValueChanged.AddListener(delegate { ChangeMusicaVolume(); });
        sensibilidadeMouseSlider.onValueChanged.AddListener(delegate { ChangeMouseSensitivity(); });

        // Set initial sensitivity
        ChangeMouseSensitivity();
    }

    public void ChangeMusicaVolume()
    {
        mainAudioMixer.SetFloat("Musica", musicaVol.value);
    }

    public void ChangeMouseSensitivity()
    {
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = sensibilidadeMouseSlider.value;
    }
}
