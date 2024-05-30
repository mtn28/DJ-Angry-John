using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        // Destroi a bala 1 segundo após sua criação
        Destroy(gameObject, 1f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroi a bala imediatamente ao colidir com algo
        Destroy(gameObject);
    }
}
