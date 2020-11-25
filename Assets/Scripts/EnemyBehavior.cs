using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    public Player player;
    public Rigidbody rb;
    public Animator animator;
    public float lookRadius = 10f;
    public int damage = 10;
    public float attackRate = 15f;

    private NavMeshAgent agent;
    private float nextTimeToAttack = 0f;

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void AttackTarget()
    {
        if (Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
            animator.SetBool("isAttacking", true);
            player.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            animator.SetBool("isAware", true);
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance + 0.5f) 
            {
                FaceTarget();
                AttackTarget();
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            animator.SetBool("isAware", false);
            agent.SetDestination(transform.position);
        }
    }
}
