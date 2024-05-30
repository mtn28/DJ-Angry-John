using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public GameObject jump; // Referência ao GameObject do salto
    public AudioSource jumpSound; // Referência ao componente AudioSource
    public AudioClip jumpClip; // Referência ao AudioClip do som de salto

    [HideInInspector]
    public bool isJumping = false; // Variável para rastrear se o jogador está pulando

    // Start is called before the first frame update
    void Start()
    {
        jump.SetActive(false); // Desativa o GameObject do salto inicialmente
        if (jumpSound != null && jumpClip != null) // Verifica se o AudioSource e o AudioClip estão configurados
        {
            jumpSound.clip = jumpClip; // Define o som de salto para o AudioSource
        }
        else
        {
            Debug.LogWarning("AudioSource ou AudioClip não está configurado corretamente."); // Avisa se houver problemas de configuração
        }

        if (!jumpSound.enabled) // Garante que o AudioSource esteja habilitado
        {
            jumpSound.enabled = true; // Ativa o AudioSource se ele estiver desativado
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Verifica se a tecla de espaço foi pressionada
        {
            StartJump(); // Inicia o pulo
        }
    }

    void StartJump()
    {
        if (!isJumping) // Verifica se o jogador não está pulando para evitar múltiplos sons simultâneos
        {
            isJumping = true; // Marca que o jogador está pulando
            jump.SetActive(true); // Ativa o GameObject do salto
            if (jumpSound.enabled) // Garante que o AudioSource esteja habilitado antes de tocar
            {
                jumpSound.Play(); // Reproduz o som de salto
            }
            StartCoroutine(StopJumpAfterDelay()); // Inicia a rotina para desativar o salto após um atraso
        }
    }

    IEnumerator StopJumpAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Espera por 1 segundo
        isJumping = false; // Marca que o jogador não está mais pulando
        jump.SetActive(false); // Desativa o GameObject do salto
    }
}
