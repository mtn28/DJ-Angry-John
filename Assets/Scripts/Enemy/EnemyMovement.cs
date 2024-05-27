using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
        
        animator.SetFloat("speed", speed);

        lastPosition = currentPosition;
    }
}
