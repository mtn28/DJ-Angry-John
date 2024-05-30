using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public GameObject footstep;
    public AudioSource footstepSound;
    public AudioClip footstepClip;

    // Variáveis para rastrear quais teclas estão pressionadas
    private bool isMovingUp = false;
    private bool isMovingDown = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
        footstepSound.loop = true; // Habilita o loop para o som dos passos
    }

    // Update is called once per frame
    void Update()
    {
        // Movimento para cima
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            isMovingUp = true;
            footsteps();
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            isMovingUp = false;
            CheckAllKeysReleased();
        }

        // Movimento para baixo
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            isMovingDown = true;
            footsteps();
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            isMovingDown = false;
            CheckAllKeysReleased();
        }

        // Movimento para a esquerda
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isMovingLeft = true;
            footsteps();
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            isMovingLeft = false;
            CheckAllKeysReleased();
        }

        // Movimento para a direita
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isMovingRight = true;
            footsteps();
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            isMovingRight = false;
            CheckAllKeysReleased();
        }
    }

    void footsteps()
    {
        // Verifica se pelo menos uma tecla de movimento está sendo pressionada
        if (isMovingUp || isMovingDown || isMovingLeft || isMovingRight)
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
        // Verifica se todas as teclas de movimento foram soltas
        if (!isMovingUp && !isMovingDown && !isMovingLeft && !isMovingRight)
        {
            footstep.SetActive(false);
            footstepSound.Stop();
        }
    }
}
