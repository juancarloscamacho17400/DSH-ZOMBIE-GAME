using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispararBala : MonoBehaviour
{
    public Transform exit;
    public GameObject bullet;
    public float shootCooldown = 100;
    public AudioClip m_GunshotSound;
    public AudioClip m_ReloadSound;
    public AudioClip m_NoAmmoSound;
    private AudioSource m_AudioSource;
    public ParticleSystem muzzleFlash;
    public Animator m_Animator;
    public int maxAmmo;
    private int ammo;
    public float reloadTime;
    private float nextShot = 0f;
    public Text ammoDisplay;
    private bool isReloading = false;
    // Start is called before the first frame update
    void Start()
    {
        ammoDisplay.text = maxAmmo.ToString() + "/" + maxAmmo.ToString();
        ammo = maxAmmo;
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading)
            return;
        if (Input.GetMouseButtonDown(0)){
            Shoot();
            return;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(Reload());
            return;
        }
    }

    IEnumerator Reload()
    {
        if(ammo < maxAmmo){
            isReloading = true;
            m_AudioSource.PlayOneShot(m_ReloadSound);
            //ANIMACION ANIMACION ANIMACION ANIMACION ANIMACION ANIMACION ANIMACION ANIMACION 
            yield return new WaitForSeconds(reloadTime);
            ammoDisplay.text = maxAmmo.ToString() + "/" + maxAmmo.ToString();
            ammo = maxAmmo;
            isReloading = false;
        }
    }

    public void Shoot(){
        if (Time.time > nextShot && ammo > 0){
            m_Animator.SetTrigger("Shoot");
            muzzleFlash.Play();
            m_AudioSource.PlayOneShot(m_GunshotSound);
            nextShot = Time.time + (shootCooldown / 1000);
            GameObject newBullet = Instantiate(bullet, exit.position, exit.rotation);
            ammo--;
            ammoDisplay.text = ammo.ToString() + "/" + maxAmmo.ToString();
        }
        else if (ammo == 0)
            m_AudioSource.PlayOneShot(m_NoAmmoSound);
    }
}
