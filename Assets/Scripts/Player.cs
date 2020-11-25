using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerHealthBar healthBar;
    public Camera fpsCam;
    public Gun gun;
    public GameObject enemyContainer; //ERROR when destroying transform component remains
    List<Transform> enemies; //ERROR when destroying transform component remains
    public int maxHealth = 100;
    public float pickupRange = 10f;
    
    private int currentHealth;
    private bool pickup = false;
    public void TakeDamage(int health)
    {
        currentHealth -= health;
        healthBar.SetHealth(currentHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        enemies = new List<Transform>();
        for (int i = 0; i < enemyContainer.transform.childCount; i++)
        {
            enemies.Add(enemyContainer.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            pickup = true;
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickupRange))
        {
            GameObject magazine = hit.collider.gameObject;
            if (magazine.CompareTag("Magazine") && pickup)
            {
                Destroy(magazine);
                gun.maxAmmo += gun.magAmmo;
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject.SetActive(false);
        }

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            GameObject enemy = hit.collider.gameObject;
            if (enemy.CompareTag("Enemy"))
            {
                var enemyHealthBarSprite = enemy.GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
                enemyHealthBarSprite.SetActive(true);
            }
        }

        pickup = false;

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }*/
    }
}
