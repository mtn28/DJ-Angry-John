using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProjectileGunTutorial : MonoBehaviour
{
    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;

    //bools
    public bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlashPrefab; // Prefab do muzzle flash
    public Transform muzzleFlashPosition;
    public TextMeshProUGUI ammunitionDisplay;

    public AudioSource audioSource; // Componente de áudio
    public AudioClip shootSound; // Som de disparo
    public AudioClip emptyGunSound; // Som de arma vazia

    public Slider reloadSlider; // Barra de recarga

    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        reloadSlider.gameObject.SetActive(false); // Desativa a barra de recarga inicialmente
    }

    private void OnEnable()
    {
        if (reloading)
        {
            reloadSlider.gameObject.SetActive(true);
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (reloading)
        {
            reloadSlider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "");
    }

    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reload automaticamente quando as balas chegarem a 0
        if (bulletsLeft <= 0 && !reloading)
        {
            Reload();
        }

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
        // Tentar disparar durante a recarga
        else if (shooting && reloading)
        {
            PlayEmptyGunSound();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Cria o muzzle flash e garante que ele está na direção correta
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, muzzleFlashPosition.position, muzzleFlashPosition.rotation);
        muzzleFlash.transform.SetParent(muzzleFlashPosition); // Anexa o muzzle flash ao ponto de spawn
        Destroy(muzzleFlash, 0.2f); // Destrói o muzzle flash após 0.2 segundos

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        audioSource.PlayOneShot(shootSound); // Toca o som de disparo
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        reloadSlider.gameObject.SetActive(true); // Ativa a barra de recarga

        // StartCoroutine para a recarga
        StartCoroutine(ReloadCoroutine());
    }

    public IEnumerator ReloadCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            reloadSlider.value = elapsedTime / reloadTime; // Atualiza a barra de recarga
            yield return null;
        }

        bulletsLeft = magazineSize;
        reloading = false;
        reloadSlider.gameObject.SetActive(false); // Desativa a barra de recarga
    }

    private void PlayEmptyGunSound()
    {
        audioSource.PlayOneShot(emptyGunSound); // Toca o som de arma vazia
    }
}
