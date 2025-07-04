using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshMovement : MonoBehaviour
{
    public string targetTag = "Player";
    public float detectionRadius = 10f;
    public float wanderRadius = 10f;

    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;

    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;

    private float walkThreshold = 0.1f; // Velocidad m�nima para activar animaci�n de caminar
    private float runThreshold = 3f;    // Velocidad m�nima para activar animaci�n de correr

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            Debug.LogWarning("No se encontr� un objeto con el tag: " + targetTag);
        }

        agent.speed = wanderSpeed;
        MoveToNewPosition();
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= detectionRadius)
            {
                agent.speed = chaseSpeed;
                agent.SetDestination(target.transform.position);
            }
            else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.speed = wanderSpeed;
                MoveToNewPosition();
            }
        }

        UpdateAnimation();
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
            // No detener el movimiento
        }
    }
}
  