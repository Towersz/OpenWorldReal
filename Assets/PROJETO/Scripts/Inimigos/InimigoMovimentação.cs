using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class InimigoMovimentação : MonoBehaviour
{
    [SerializeField]
    private Transform[] PontosdePatrulha;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float DetecçãoAlcance = 10f;
    [SerializeField]
    private float AtaqueRange = 5f;

    private NavMeshAgent agent;
    private int PatrulhaAtualIndex;
    private InimigoCombate combate;
    private InimigoSaude saude;

    private enum Estado { Patrulha, Chase, Ataque}
    private Estado atualEstado = Estado.Patrulha;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        combate = GetComponent<InimigoCombate>();
        saude = GetComponent<InimigoSaude>();
        PatrulhaAtualIndex = 0;

        IrproProximoPontosdePatrulha();
    }

    void Update()
    {
        if (saude.Die()) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= AtaqueRange)
        {
            atualEstado = Estado.Ataque;
        }
        else if (distance <= DetecçãoAlcance)
        {
            atualEstado = Estado.Chase;
        }
        else
        {
            atualEstado = Estado.Patrulha;
        }

        switch (atualEstado)
        {
            case Estado.Patrulha:
                Patrulha();
                break;
            case Estado.Chase:
                Chaseplayer();
                break;
            case Estado.Ataque:
                Ataqueplayer();
                break;

        }
    }

    void Patrulha()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            IrproProximoPontosdePatrulha();
        }
    }
    void IrproProximoPontosdePatrulha()
    {
        if (PontosdePatrulha.Length == 0) return;
        agent.destination = PontosdePatrulha[PatrulhaAtualIndex].position;
        PatrulhaAtualIndex = (PatrulhaAtualIndex + 1) % PontosdePatrulha.Length;
    }
    void Chaseplayer()
    {
        agent.SetDestination(player.position);
    }
    void Ataqueplayer()
    {
        agent.ResetPath();
        transform.LookAt(player);
        combate.TryAtaque(player.gameObject);
    }
}
