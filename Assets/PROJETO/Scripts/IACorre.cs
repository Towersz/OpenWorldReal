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
    private float lastWanderTime = 0f;

    public enum States
    {
        patrulha,
        foge
    }

    public States states;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!player) Debug.LogWarning("Jogador não encontrado. Certifique-se de que ele tem a tag 'Player'.");
        StartCoroutine("Patrulha");
    }

  void Update()
{
    
}

    void StateMachine(States _states)
    {
        states = _states;
        switch (states)
        {
            case States.patrulha:
                StartCoroutine(Patrulha()); 
                break;
            case States.foge:
                StartCoroutine(Foge());
                break;  
        }
    }
    Vector3 RandomNav(Vector3 origem, float raio)
    {
        for (int i = 0; i < 30; i++) // tenta encontrar um ponto válido até 30 vezes
        {
            Vector3 direcaoAleatoria = Random.insideUnitSphere * raio;
            direcaoAleatoria += origem;
            if (NavMesh.SamplePosition(direcaoAleatoria, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return origem; // se não achar nenhum ponto, retorna onde está
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
            // Direção oposta ao jogador
            Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

            // Adiciona aleatoriedade ao ângulo da fuga
            float randomAngle = Random.Range(-90f, 90f);
            directionAwayFromPlayer = Quaternion.Euler(0, randomAngle, 0) * directionAwayFromPlayer;

            // Calcula ponto de fuga
            Vector3 fleeTarget = transform.position + directionAwayFromPlayer * fleeDistance;

            // Garante que o ponto de fuga está em uma área válida da NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, 5f, NavMesh.AllAreas))
            {
                // Define o destino correto
                agent.SetDestination(hit.position);

                // Aguarda até o agente chegar ao ponto de fuga (com margem)
                while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null; // Espera um frame
                }

                // Quando chega no ponto de fuga, volta para o estado anterior (ex: patrulha)
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
            // Jogador longe, nada a fazer
            StateMachine(States.patrulha);
        }
    }
}

  
