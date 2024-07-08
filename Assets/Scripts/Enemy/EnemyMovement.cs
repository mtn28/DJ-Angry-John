using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;
    private AudioSource moveAudioSource;
    public float moveSoundVolume = 1.0f; // Volume base do som de movimento
    private bool isMoving = false;
    public Transform player; // Referência ao jogador
    public float maxDistance = 10.0f; // Distância máxima para o som ser audível

    public AudioSource externalAudioSource; // Referência ao AudioSource externo

    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;

        // Use o AudioSource externo se disponível, caso contrário, adicione um novo AudioSource
        if (externalAudioSource != null)
        {
            moveAudioSource = externalAudioSource;
        }
        else
        {
            moveAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (moveAudioSource != null)
        {
            moveAudioSource.loop = true; // Certifique-se de que o som está em loop
            moveAudioSource.volume = 0f; // Inicie com o volume zero
            moveAudioSource.playOnAwake = false; // Desative Play On Awake
        }

        yield return new WaitForSeconds(6f); // Aguarde 6 segundos antes de iniciar a verificação da velocidade

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

            if (isMoving) // Ajuste o volume do som conforme a proximidade ao jogador
            {
                float distance = Vector3.Distance(transform.position, player.position);
                float volume = Mathf.Clamp(1 - (distance / maxDistance), 0, 1) * moveSoundVolume;
                moveAudioSource.volume = volume;
            }

            lastPosition = currentPosition;

            yield return new WaitForSeconds(0.1f); // Verifique a velocidade a cada 0.1 segundos
        }
    }
}
