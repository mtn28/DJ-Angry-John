using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public GameObject footstep;
    public AudioSource footstepSound;
    public AudioClip footstepClip;
    public Jump jumpScript; // Referência ao script de salto

    // Variáveis para rastrear quais teclas estão pressionadas
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Movimento para cima
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            isMovingUp = true;
            footsteps();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            isMovingUp = false;
            CheckAllKeysReleased();
        }

        // Movimento para baixo
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            isMovingDown = true;
            footsteps();
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            isMovingDown = false;
            CheckAllKeysReleased();
        }

        // Movimento para a esquerda
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isMovingLeft = true;
            footsteps();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            isMovingLeft = false;
            CheckAllKeysReleased();
        }

        // Movimento para a direita
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isMovingRight = true;
            footsteps();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            isMovingRight = false;
            CheckAllKeysReleased();
        }

        // Parar os passos se estiver pulando
        if (jumpScript.isJumping)
        {
            StopFootsteps();
        }
    }

    void footsteps()
    {
        // Verifica se pelo menos uma tecla de movimento está sendo pressionada e o jogador não está pulando
        if ((isMovingUp || isMovingDown || isMovingLeft || isMovingRight) && !jumpScript.isJumping)
        {
            footstep.SetActive(true);
            if (!footstepSound.isPlaying)
            {
                footstepSound.clip = footstepClip;
                footstepSound.Play();
            }
        }
    }

    void CheckAllKeysReleased()
    {
        // Verifica se todas as teclas de movimento foram soltas ou o jogador está pulando
        if ((!isMovingUp && !isMovingDown && !isMovingLeft && !isMovingRight) || jumpScript.isJumping)
        {
            StopFootsteps();
        }
    }

    void StopFootsteps()
    {
        footstep.SetActive(false);
        if (footstepSound.isPlaying)
        {
            footstepSound.Stop();
        }
    }
}
