using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public AudioSource gunshotSound;
    public AudioSource reloadSound1;
    public AudioSource reloadSound2;
    public AudioSource reloadSound3;
    public RawImage crosshair;
    public Animator animator;
    public Vector3 recoilRotationHipfire = new Vector3(2f, 2f, 2f);
    public Vector3 recoilRotationADS = new Vector3(0.5f, 0.5f, 1.5f);
    public Vector3 ADS_Position=new Vector3(0f, 0f, 0f);
    public int damage = 20;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public float rotationSpeed = 6f;
    public float returnSpeed = 25f;
    public float adsSpeed = 8f;
    public int maxAmmo = 90;
    public int magAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1f;

    private Vector3 currentRotation;
    private Vector3 rot;
    private Vector3 hipfirePosition;
    private int defaultDamage;
    private float nextTimeToFire = 0f;
    private bool ADS = false;
    private bool isReloading = false;

    void AimDownSights(bool ADS)
    {
        if (ADS && !isReloading)
        {
            crosshair.enabled = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, ADS_Position, Time.deltaTime * adsSpeed);
        }
        else
        {
            crosshair.enabled = true;
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipfirePosition, Time.deltaTime * adsSpeed);
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
            return;

        muzzleFlash.Play();
        gunshotSound.Play();
        currentAmmo--;

        if (ADS)
        {
            damage = defaultDamage * 2;
            currentRotation += new Vector3(-recoilRotationADS.x, Random.Range(-recoilRotationADS.y, recoilRotationADS.y), Random.Range(-recoilRotationADS.z, recoilRotationADS.z));
        }
        else
        {
            damage = defaultDamage;
            currentRotation += new Vector3(-recoilRotationHipfire.x, Random.Range(-recoilRotationHipfire.y, recoilRotationHipfire.y), Random.Range(-recoilRotationHipfire.z, recoilRotationHipfire.z));
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                //DOESN'T WORK
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    void ReloadSound()
    {
        bool hasPlayed1 = false;
        bool hasPlayed2 = false;
        bool hasPlayed3 = false;

        if (!hasPlayed1)
        {
            reloadSound1.Play();
            hasPlayed1 = true;
        }
        if(!hasPlayed2)
        {
            reloadSound2.Play();
            hasPlayed2 = true;
        }
        if (!hasPlayed3)
        {
            reloadSound3.Play();
            hasPlayed3 = true;
        }
    }

    IEnumerator Reload()
    {
        if (maxAmmo <= 0 || currentAmmo == magAmmo)
        {
            isReloading = false;
            animator.SetBool("Reloading", false);
            yield return null;
        }
        else
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
            ReloadSound();

            yield return new WaitForSeconds(reloadTime - 0.25f);
            isReloading = false;
            yield return new WaitForSeconds(0.25f);

            int ammoDifference = magAmmo - currentAmmo;
            ammoDifference = maxAmmo >= ammoDifference ? ammoDifference : maxAmmo;
            maxAmmo -= ammoDifference;
            currentAmmo += ammoDifference;

            animator.SetBool("Reloading", false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        defaultDamage = damage;
        currentAmmo = magAmmo;
        hipfirePosition = transform.localPosition;
    }
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ADS = !ADS;
        }

        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(Reload());
        }

        AimDownSights(ADS);
    }

    void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rot);
    }
}
