using UnityEngine;

public class InimigoCombate : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float Cooldown = 2f;

    private float Ultimoataque = 0f;

    private InimigoAnimator animator;

    private void Start()
    {
        animator = GetComponent<InimigoAnimator>();
    }

    public void TryAtaque(GameObject target)
    {
        if (Time.time >= Ultimoataque)
        {
            Ultimoataque = Time.time + Cooldown;
            animator.Ataque();
        }
    }

    public void AplicarDano()
    {
        Debug.Log("Dano Aplicado");
    }
}
