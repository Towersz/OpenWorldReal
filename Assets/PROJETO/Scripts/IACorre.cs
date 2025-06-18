using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IAFoge : MonoBehaviour
{
    public NavMeshAgent agent;
    public float detectionRadius = 10f;
    public float fleeDistance = 15f;
    public float wanderRadius = 20f;

    public Animator anim;

    private GameObject player;
    private bool isDead = false;

    public string enemyType;

    public enum States
    {
        patrulha,
        foge,
        morre
    }

    public States states;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!player) Debug.LogWarning("Jogador não encontrado. Certifique-se de que ele tem a tag 'Player'.");
        StateMachine(States.patrulha);
    }

    void Update()
    {
        // Exemplo: tecla para simular morte (apenas para teste)
        if (Input.GetKeyDown(KeyCode.K) && !isDead)
        {
            Morre();
        }
    }

    void StateMachine(States _states)
    {
        states = _states;
        StopAllCoroutines(); // Interrompe o estado anterior
        switch (states)
        {
            case States.patrulha:
                StartCoroutine(Patrulha());
                break;
            case States.foge:
                StartCoroutine(Foge());
                break;
            case States.morre:
                StartCoroutine(Morte());
                break;
        }
    }

    Vector3 RandomNav(Vector3 origem, float raio)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 direcaoAleatoria = Random.insideUnitSphere * raio;
            direcaoAleatoria += origem;
            if (NavMesh.SamplePosition(direcaoAleatoria, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return origem;
    }

    private IEnumerator Patrulha()
    {
        agent.isStopped = false;
        agent.speed = 3;
        agent.destination = RandomNav(transform.position, wanderRadius);

        anim.SetBool("corre", false);
        yield return new WaitForSeconds(1);

        if (Vector3.Distance(transform.position, player.transform.position) < detectionRadius * 2)
        {
            StateMachine(States.foge);
        }
        else
        {
            StateMachine(States.patrulha);
        }
    }

    private IEnumerator Foge()
    {
        agent.isStopped = false;
        agent.speed = 7;
        anim.SetBool("corre", true);

        yield return new WaitForSeconds(1f);

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;
            float randomAngle = Random.Range(-90f, 90f);
            directionAwayFromPlayer = Quaternion.Euler(0, randomAngle, 0) * directionAwayFromPlayer;

            Vector3 fleeTarget = transform.position + directionAwayFromPlayer * fleeDistance;

            if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);

                while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }

                StateMachine(States.patrulha);
            }
            else
            {
                Debug.Log("Ponto de fuga inválido. Retornando à patrulha.");
                StateMachine(States.patrulha);
            }
        }
        else
        {
            StateMachine(States.patrulha);
        }
    }

    private IEnumerator Morte()
    {
        agent.isStopped = true;
        anim.SetTrigger("morre");

        // Espera a animação terminar (ajuste o tempo conforme sua animação)
        yield return new WaitForSeconds(2f);

        Destroy(gameObject); // OU desativa: gameObject.SetActive(false);
    }

    public void Morre()
    {
        if (!isDead)
        {
            isDead = true;

            //  Notifica a missão
            QuestManager.Instance.EnemyKilled(enemyType);

            StateMachine(States.morre);
        }
    }
}


