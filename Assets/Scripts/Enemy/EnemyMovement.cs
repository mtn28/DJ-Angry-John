using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;
    public AudioClip moveSound;
    private AudioSource moveAudioSource;
    public float moveSoundVolume = 1.0f; // Adicione esta linha para controlar o volume
    private bool isMoving = false; // Adicione esta linha

    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        moveAudioSource = gameObject.AddComponent<AudioSource>();

        if (moveAudioSource != null)
        {
            moveAudioSource.clip = moveSound;
            moveAudioSource.loop = true; // Certifique-se de que o som está em loop
            moveAudioSource.volume = moveSoundVolume; // Defina o volume do som de movimento
            moveAudioSource.playOnAwake = false; // Desative Play On Awake
        }

        yield return new WaitForSeconds(6f); // Aguarde 1 segundo antes de iniciar a verificação da velocidade

        StartCoroutine(CheckSpeed());
    }

    IEnumerator CheckSpeed()
    {
        while (true)
        {
            Vector3 currentPosition = transform.position;
            float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;

            animator.SetFloat("speed", speed);

            if (speed > 0.1f && !isMoving) // Inicie o som quando o inimigo começar a se mover
            {
                isMoving = true;
                moveAudioSource.Play();
            }
            else if (speed <= 0.1f && isMoving) // Pare o som quando o inimigo parar de se mover
            {
                isMoving = false;
                moveAudioSource.Stop();
            }

            lastPosition = currentPosition;

            yield return new WaitForSeconds(0.1f); // Verifique a velocidade a cada 0.1 segundos
        }
    }
}
