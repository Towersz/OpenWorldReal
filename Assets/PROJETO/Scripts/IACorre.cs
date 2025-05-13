using UnityEngine;
using UnityEngine.AI;

public class IAFoge : MonoBehaviour
{
    public NavMeshAgent agent;
    public float detectionRadius = 10f;
    public float fleeDistance = 15f;
    public float wanderRadius = 20f;
    public float wanderCooldown = 5f;
    public Animator anim;

    private GameObject player;
    private float lastWanderTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!player) Debug.LogWarning("Jogador não encontrado. Certifique-se de que ele tem a tag 'Player'.");
    }

  void Update()
{
    if (!player) return;

    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

    if (distanceToPlayer < detectionRadius)
    {
        // FUGA COM DIREÇÃO ALEATÓRIA (mas afastando do jogador)
        Vector3 fleeDirection = (transform.position - player.transform.position).normalized;

        // Gira a direção com um desvio aleatório de até 90 graus
        float randomAngle = Random.Range(-90f, 90f);
        fleeDirection = Quaternion.Euler(0, randomAngle, 0) * fleeDirection;

        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        agent.speed = 10;
        anim?.SetBool("corre", true);
    }
    else
    {
        // PATRULHA ALEATÓRIA
        agent.speed = 3;
        anim?.SetBool("corre", false);

        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            if (Time.time - lastWanderTime > wanderCooldown)
            {
                Vector3 randomDestination = RandomNavMeshPoint(wanderRadius);
                agent.SetDestination(randomDestination);
                lastWanderTime = Time.time;
            }
        }
    }
}


    Vector3 RandomNavMeshPoint(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas);

        return navHit.position;
    }
}
