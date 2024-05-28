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
    public Camera playerCamera; // Referência à câmera do jogador

    void Update()
    {
        // Verifica se o tempo atual é maior que o tempo do próximo disparo
        if (Time.time >= nextFireTime)
        {
            // Se o botão esquerdo do mouse for pressionado
            if (Input.GetMouseButtonDown(0))
            {
                // Define o tempo do próximo disparo como o tempo atual mais o intervalo de tempo entre os disparos
                nextFireTime = Time.time + fireRate;
                GameObject Muzzle = Instantiate(muzzleFlash, muzzleFlashPosition);
                Destroy(Muzzle, 0.2f);
                
                // Raycast a partir da câmera
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 targetPoint;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(1000); // Algum ponto distante se não houver colisão
                }

                Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;

                // Instancia a bala e define sua velocidade
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
                bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            }
        }
    }
}
