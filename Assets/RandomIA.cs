using UnityEngine;
using UnityEngine.AI;

public class RamdomIa:MonoBehaviour
{
    public float raioDePatrulha = 20f;
    public float tempoDeEspera = 2f;
    public float distanciaMinimaParaDestino = 1f;

    private NavMeshAgent agent;
    private float cronometroEspera = 0f;
    private bool esperando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        EscolherNovoDestino();
    }

    void Update()
    {
        // Chegou ao destino
        if (!esperando && !agent.pathPending && agent.remainingDistance <= distanciaMinimaParaDestino)
        {
            esperando = true;
            cronometroEspera = tempoDeEspera;
            agent.ResetPath();
        }

        // Esperando antes de escolher novo ponto
        if (esperando)
        {
            cronometroEspera -= Time.deltaTime;
            if (cronometroEspera <= 0f)
            {
                esperando = false;
                EscolherNovoDestino();
            }
        }
    }

    void EscolherNovoDestino()
    {
        Vector3 destinoAleatorio = RandomNavMeshPoint(transform.position, raioDePatrulha);
        agent.SetDestination(destinoAleatorio);
    }

    Vector3 RandomNavMeshPoint(Vector3 origem, float raio)
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
}
