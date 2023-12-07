using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public GameObject home;
    public GameObject projectilePrefab;
    public Transform startPoint;
    public Transform endPoint;

    private NavMeshAgent navMeshAgent;
    private Rigidbody projectileRigidbody;

    private bool isAttacking = false;
    private float attackRange = 10f;
    private float attackCooldown = 2f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        projectileRigidbody = projectilePrefab.GetComponent<Rigidbody>();

        // Set the bot's starting position
        navMeshAgent.SetDestination(startPoint.position);
    }

    void Update()
    {
        // Check if the bot has reached the ending point
        if (Vector3.Distance(transform.position, endPoint.position) <= 0.5f)
        {
            // Stop moving and destroy the bot
            navMeshAgent.isStopped = true;
            Destroy(gameObject);
        }
        else
        {
            // Check attacking condition
            if (!isAttacking)
            {
                if (IsHomeInAttackRange())
                {
                    StopAndAttack();
                }
                else
                {
                    // Set a constant speed for smooth movement
                    navMeshAgent.speed = 5f;
                    // Continue moving towards the ending point
                    navMeshAgent.SetDestination(endPoint.position);
                }
            }
        }
    }

    bool IsHomeInAttackRange()
    {
        float distanceToHome = Vector3.Distance(transform.position, home.transform.position);
        // Check if the distance to home is less than the attack range but greater than a minimum threshold
        return distanceToHome < attackRange && distanceToHome > 2f;
    }

    void StopAndAttack()
    {
        // Stop moving immediately
        navMeshAgent.speed = 0f;
        StartCoroutine(AttackWithCooldown());
    }

    IEnumerator AttackWithCooldown()
    {
        isAttacking = true;
        Attack();
        yield return new WaitForSeconds(attackCooldown);
        // Khôi phục tốc độ ban đầu của bot
        navMeshAgent.speed = 0f;
        isAttacking = false;
    }

    void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        // Throw projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        // Calculate throwing force
        Vector3 attackDirection = (home.transform.position - transform.position).normalized;
        float projectileSpeed = 15f; // Điều chỉnh tốc độ của đạn

        // Launch the projectile
        projectileRB.velocity = attackDirection * projectileSpeed;
        float distanceToHome = Vector3.Distance(projectile.transform.position, home.transform.position);
        if (distanceToHome < 2f)
        {
            // Destroy the projectile on collision with home
            Destroy(projectile);
        }
        yield return null;
    }
}
