using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public float fireRate = 1.0f;
    public int maxBullets = 10;
    public float reloadTime = 5.0f;
    public GameObject muzzleFlash;
    public Transform muzzleFlashPosition;
    public Camera playerCamera;
    public TextMeshProUGUI ammoText; // Referência ao TextMeshPro no HUD
    public Slider reloadSlider; // Barra de recarga
    public AudioSource audioSource; // Componente de áudio
    public AudioClip shootSound; // Som de disparo
    public AudioClip emptyGunSound; // Som de arma vazia

    private float nextFireTime = 0.0f;
    private int bulletsFired = 0;
    private bool isReloading = false;

    void Start()
    {
        UpdateAmmoText();
        reloadSlider.gameObject.SetActive(false); // Desativa a barra de recarga inicialmente
    }

    void Update()
    {
        if (isReloading)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayEmptyGunSound();
            }
            return;
        }

        if (Time.time >= nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Verificar se o clique não foi em um elemento de UI
                if (!IsPointerOverUI())
                {
                    if (bulletsFired < maxBullets)
                    {
                        FireBullet();
                        bulletsFired++;
                        UpdateAmmoText(); // Atualiza o texto após cada disparo

                        if (bulletsFired >= maxBullets)
                        {
                            StartCoroutine(Reload());
                        }
                        else
                        {
                            nextFireTime = Time.time + fireRate;
                        }
                    }
                    else
                    {
                        PlayEmptyGunSound();
                    }
                }
            }
        }
    }

    private void FireBullet()
    {
        GameObject Muzzle = Instantiate(muzzleFlash, muzzleFlashPosition.position, muzzleFlashPosition.rotation);
        Destroy(Muzzle, 0.2f);

        Vector3 forwardDirection = playerCamera.transform.forward;

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(forwardDirection));
        bullet.GetComponent<Rigidbody>().velocity = forwardDirection * bulletSpeed;

        audioSource.PlayOneShot(shootSound); // Toca o som de disparo
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        reloadSlider.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadSlider.value = elapsedTime / reloadTime; // Atualiza a barra de recarga
            yield return null;
        }

        bulletsFired = 0;
        nextFireTime = Time.time + fireRate; // Permite disparar imediatamente após recarregar
        UpdateAmmoText(); // Atualiza o texto após recarregar
        isReloading = false;
        reloadSlider.gameObject.SetActive(false); // Desativa a barra de recarga
    }

    private void UpdateAmmoText()
    {
        ammoText.text = (maxBullets - bulletsFired).ToString();
    }

    private void PlayEmptyGunSound()
    {
        audioSource.PlayOneShot(emptyGunSound); // Toca o som de arma vazia
    }

    // Método para verificar se o ponteiro está sobre um elemento de UI
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
