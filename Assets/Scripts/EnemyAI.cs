using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Vitals;
using static Unity.VisualScripting.Member;
using static UnityEngine.EventSystems.EventTrigger;


public class EnemyAI : MonoBehaviour
{
    Animator enemyAnim;
    public float enemyHealth = 100;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public static float speed = 0;

    public AudioSource attackSound;
    public AudioSource walkSound;
    public AudioSource runSound;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;

    public CapsuleCollider col;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    CharacterData chrData;
    DamageEffect dmgEffect;

    public float cooldown = 1f;
    private float lastAttackedAt = -9999f;
    public GameObject teleportArea;
    public GameObject teleportArea_1;


    public enum AttackerStates
    {
        Patrol,
        Chase,
        Attack,
        Die
    }

    public AttackerStates AttackerCurrentState = AttackerStates.Patrol;

    private void Awake()
    {
        enemyAnim = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>(); 
        dmgEffect = FindObjectOfType<DamageEffect>();
        chrData = FindObjectOfType<CharacterData>();
        agent.speed= speed;
        
        walkSound.Stop();
        runSound.Stop();
        attackSound.Stop();

    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(AttackerCurrentState != AttackerStates.Die)
        {
            if (!playerInSightRange && !playerInAttackRange) AttackerCurrentState = AttackerStates.Patrol;
            if (playerInSightRange && !playerInAttackRange) AttackerCurrentState = AttackerStates.Chase;
            if (playerInSightRange && playerInAttackRange) AttackerCurrentState = AttackerStates.Attack;
        }

        switch (AttackerCurrentState)
        {
            case AttackerStates.Patrol:
                enemyAnim.SetBool("IsAttacking", false);
                enemyAnim.SetBool("IsRunning", false);
                enemyAnim.SetBool("IsWalking", true);
                agent.speed = 0.5f;
                if (!walkPointSet) SearchWalkPoint();
                if (walkPointSet)
                {
                    PlaySound();
                    agent.SetDestination(walkPoint);
                    enemyAnim.SetBool("IsWalking", agent.velocity.magnitude > 0.01);
                }
                Vector3 distanceToWalkPoint = transform.position - walkPoint;
                if (distanceToWalkPoint.magnitude < 2f)
                    walkPointSet = false;
                break;
            case AttackerStates.Chase:
                enemyAnim.SetBool("IsAttacking", false);
                agent.speed = 2.5f;
                enemyAnim.SetBool("IsRunning", true);
                enemyAnim.SetBool("IsWalking", false);

                PlaySound();
                Vector3 lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookPosition);
                agent.SetDestination(player.position);
                break;
            case AttackerStates.Attack:
                agent.speed = 2.5f;
                if (Time.time > lastAttackedAt + cooldown)
                {
                    Vector3 lPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
                    transform.LookAt(lPosition);
                    enemyAnim.SetBool("IsAttacking", true);
                    enemyAnim.SetBool("IsRunning", false);

                    PlaySound();
                    agent.SetDestination(player.position);

                    chrData.Damage(20);
                    Debug.Log(chrData.Health.Value.ToString());
                    Coroutine hiteffect = StartCoroutine(dmgEffect.TakeDamageEffect());
                    if (chrData.Health.Value <= 0)
                    {
                        StopCoroutine(hiteffect);

                        chrData.PlayerDie();
                    }
                    lastAttackedAt = Time.time;
                } 
                break;
            case AttackerStates.Die:
                agent.isStopped = true;
                agent.angularSpeed = 0;
                col.enabled = false;
                enemyAnim.SetBool("IsFallingBack", true);
                StartCoroutine(nameof(TeleportEnemy), 3f);
                break;
        }
    
    }

    private Vector3 RandomNavmeshPosition()
    {
        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * 7;
        NavMeshHit hit; 
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }
    private void SearchWalkPoint()
    {
        walkPoint = RandomNavmeshPosition(); 
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;  
        }
    }
    public void PlaySound()
    {
        if(!playerInSightRange && !playerInAttackRange)
        {
            if(!walkSound.isPlaying)
                walkSound.Play();
        }
        else if(playerInSightRange && !playerInAttackRange)
        {
            if (!runSound.isPlaying && !attackSound.isPlaying)
                runSound.Play();
                attackSound.Play();
        }
        else if(playerInAttackRange)
        {
            if (!attackSound.isPlaying)
                attackSound.Play();
        }
    }
    public float EnemyDamage()
    {
        enemyHealth -= 50;
        return enemyHealth;
    }
    public IEnumerator TeleportEnemy()
    {
        yield return new WaitForSeconds(3);
        AttackerCurrentState = AttackerStates.Patrol;
        enemyAnim.SetBool("IsFallingBack", false);
   
        TeleportationDistance();

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        agent.isStopped = false;
        col.enabled = true;
        agent.angularSpeed = 40; 
        enemyAnim.SetBool("IsWalking", true);

    }
    public void TeleportationDistance()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        float distance_1 = Vector3.Distance(transform.position, teleportArea.transform.position);
        float distance_2 = Vector3.Distance(transform.position, teleportArea_1.transform.position);

        if(distance_1 >= distance_2)
        {
            transform.position = teleportArea.transform.position;
        }
        else if(distance_2 > distance_1)
        {
            transform.position = teleportArea_1.transform.position;
        }
        else
        {
            transform.position = teleportArea.transform.position;

        }
    }


   


}
