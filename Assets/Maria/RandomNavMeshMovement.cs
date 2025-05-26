using System.Collections;
using System.Collections.Generic;
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
    private bool isTargetReached = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Asegúrate de tener un Animator en el mismo GameObject

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
        if (isTargetReached)
        {
            SetAnimationSpeed(0f); // Detener animación
            return;
        }

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

        // Actualiza la animación según la velocidad actual
        SetAnimationSpeed(agent.velocity.magnitude);
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
            agent.isStopped = true;
            isTargetReached = true;
            Debug.Log("Objetivo alcanzado. Se ha detenido.");
        }
    }
}
  