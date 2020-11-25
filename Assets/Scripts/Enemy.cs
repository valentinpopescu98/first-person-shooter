using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player playerScript;
    public EnemyHealthBar healthBar;
    public Animator animator;
    public int maxHealth = 100;

    int currentHealth;
    bool dead=false;

    IEnumerator Die()
    {
        animator.SetTrigger("isDead");
        dead = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !dead)
        {
            StartCoroutine(Die());
        }
    }
}
