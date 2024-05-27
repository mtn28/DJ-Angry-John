using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public float fireRate = 1.0f;
    private float nextFireTime = 0.0f;
    public GameObject muzzleFlash;
    public Transform muzzleFlashPosition;
 
    void Update()
    {
        // Verifica se o tempo atual é maior que o tempo do próximo disparo
        if (Time.time >= nextFireTime)
        {
            // Se o botão esquerdo do mouse for pressionado
            if(Input.GetMouseButtonDown(0))
            {
                // Define o tempo do próximo disparo como o tempo atual mais o intervalo de tempo entre os disparos
                nextFireTime = Time.time + fireRate;
                GameObject Muzzle = Instantiate(muzzleFlash, muzzleFlashPosition);
                Destroy(Muzzle, 0.2f);
                // Instancia a bala e define sua velocidade
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
        }
    }
}
