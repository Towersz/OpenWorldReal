using UnityEngine;
using UnityEngine.AI;

public class IAPatrol: MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;

    public float distanciaDeTroca = 1f;
    public float esperaNoDestino = 1f;
    public Animator anim;

    private NavMeshAgent agent;
    private Transform destinoAtual;
    private float tempoDeEspera = 0f;
    private bool esperando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destinoAtual = pontoA;
        agent.SetDestination(destinoAtual.position);
    }

    void Update()
    {
        // Verifica se chegou no destino
        if (!esperando && Vector3.Distance(transform.position, destinoAtual.position) <= distanciaDeTroca)
        {
            esperando = true;
            tempoDeEspera = esperaNoDestino;
            agent.ResetPath();

            // Alterna destino
            destinoAtual = destinoAtual == pontoA ? pontoB : pontoA;
        }

        // Espera parado antes de ir pro próximo ponto
        if (esperando)
        {
            tempoDeEspera -= Time.deltaTime;
            if (tempoDeEspera <= 0f)
            {
                esperando = false;
                agent.SetDestination(destinoAtual.position);
            }
        }

        // Atualiza animação, se houver
        if (anim)
        {
            anim.SetBool("corre", agent.velocity.magnitude > 0.1f && !esperando);
        }
    }
}
