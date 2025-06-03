using UnityEngine;

public class InimigoHitbox : MonoBehaviour
{
    [SerializeField]
    private int Dano = 10;

    private bool Ativado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!Ativado) return;

        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Atingido");
        }
    }
    public void HitboxAtivada()
    {
        Ativado = true;
        Debug.Log("Hitbox Ativada");
    }
    public void HitboxDesativada()
    {
        Ativado = false;
        Debug.Log("Hitbox Desativada");
    }
}
