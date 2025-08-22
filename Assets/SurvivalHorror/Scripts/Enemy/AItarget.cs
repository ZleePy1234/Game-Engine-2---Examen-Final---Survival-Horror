using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D;

[RequireComponent(typeof(NavMeshAgent))]
public class AItarget : MonoBehaviour
{
    public Transform target;
    public float attackDistance;

    public NavMeshAgent agent;
    private Animator animator;
    private float distance;
    public float returnDistance;
    private Vector3 spawnPos;
    float originalSpeed;
    public bool isAtSpawnPos;

    private bool isDead = false;

    [SerializeField] private Collider kickCollider;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        SaveSpawnPos();
        originalSpeed = agent.speed;

    }

    void Update()
    {
        AttackPlayer();
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    void SaveSpawnPos()
    {
        spawnPos = transform.position;
    }

    void AttackPlayer()
    {
        if (isDead == true)
        {
            return;
        }
        distance = Vector3.Distance(agent.transform.position, target.position);
        if (distance < attackDistance)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("kick"))
            {
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("kick"))
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }
    public void DisableKickCollider()
    {
        kickCollider.enabled = false;
    }
    public void EnableKickCollider()
    {
        kickCollider.enabled = true;
    }

    //PlayerState Implementation

    public void Idle()
    {
        isAtSpawnPos = true;
        agent.SetDestination(transform.position);
        animator.SetBool("moving", false);
        animator.SetBool("idle", true);
    }
    public void Chase()
    {
        agent.SetDestination(target.transform.position);
        animator.SetBool("idle", false);
        animator.SetBool("moving", true);
        isAtSpawnPos = false;
    }

    public void Enrage()
    {
        agent.speed *= 1.5f;
        animator.SetBool("enraged", true);
    }
    public void EnrageUndo()
    {
        agent.speed = originalSpeed;
        animator.SetBool("enraged", false);
    }
    public void TargetLastSeen()
    {
        agent.SetDestination(target.transform.position);
        animator.SetBool("idle", false);
        animator.SetBool("moving", true);
    }
    public void ReturnToSpawn()
    {
        agent.SetDestination(spawnPos);
        distance = Vector3.Distance(agent.transform.position, spawnPos);
        animator.SetBool("idle", false);
        animator.SetBool("moving", true);
        if (Vector3.Distance(transform.position, spawnPos) < returnDistance)
        {
            animator.SetBool("moving", false);
            animator.SetBool("idle", true);
            isAtSpawnPos = true;
        }
    }
    public void Dead()
    {
        isDead = true;
        animator.SetTrigger("dead");
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        agent.speed = 0;
        agent.ResetPath();
        agent.isStopped = true;
    }
}
