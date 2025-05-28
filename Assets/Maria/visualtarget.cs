using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class visualtarget : MonoBehaviour
{
    public string targetTag = "Player";
    public float detectionRadius = 10f;
    public float wanderRadius = 10f;

    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;

    public float viewAngle = 120f; // Ángulo de visión en grados
    public float viewDistance = 10f;

    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;

    private float walkThreshold = 0.1f;
    private float runThreshold = 3f;

    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            Debug.LogWarning("No se encontró un objeto con el tag: " + targetTag);
        }

        agent.speed = wanderSpeed;
        MoveToNewPosition();
    }

    void Update()
    {
        if (target != null)
        {
            if (CanSeeTarget())
            {
                isChasing = true;
                agent.speed = chaseSpeed;
                agent.SetDestination(target.transform.position);
            }
            else if (isChasing)
            {
                // Aún persigue mientras no llega al último destino conocido
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    isChasing = false;
                    agent.speed = wanderSpeed;
                    MoveToNewPosition();
                }
            }
            else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.speed = wanderSpeed;
                MoveToNewPosition();
            }
        }

        UpdateAnimation();
    }

    bool CanSeeTarget()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (angle > viewAngle / 2f)
            return false;

        // Verificar línea de visión
        if (Physics.Raycast(transform.position + Vector3.up, directionToTarget.normalized, out RaycastHit hit, viewDistance))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                return true;
            }
        }

        return false;
    }

    void UpdateAnimation()
    {
        float currentSpeed = agent.velocity.magnitude;

        if (currentSpeed > walkThreshold)
        {
            SetAnimationSpeed(currentSpeed);

            if (agent.speed == chaseSpeed && currentSpeed >= runThreshold)
            {
                SetCreatureAnimation(1f, 1f); // Correr
            }
            else
            {
                SetCreatureAnimation(1f, 0f); // Caminar
            }
        }
        else
        {
            SetAnimationSpeed(0f);
            SetCreatureAnimation(0f, 0f); // Quieto o Idle
        }
    }

    void SetCreatureAnimation(float vert, float state)
    {
        if (animator != null)
        {
            animator.SetFloat("Vert", vert);
            animator.SetFloat("State", state);
        }
    }

    void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }

    void MoveToNewPosition()
    {
        Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
        agent.SetDestination(newPos);
    }

    Vector3 GetRandomPoint(Vector3 origin, float distance)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;
            randomDirection.y = origin.y;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, distance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return origin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Jugador alcanzado.");
            // Aquí podrías activar daño o transición
        }
    }

}
