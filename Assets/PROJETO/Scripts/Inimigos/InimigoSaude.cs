using System.Linq.Expressions;
using UnityEngine;

public class InimigoSaude : MonoBehaviour
{
    [SerializeField]
    private int MaxSaude = 100;
    private int SaudeAtual;
    private InimigoAnimator animator;
    private bool isDead = false;

    private void Start()
    {
        SaudeAtual = MaxSaude;
        animator = GetComponent<InimigoAnimator>();
    }
    public void TakeDamege(int Quantidade)
    {
        if (isDead) return;

        SaudeAtual -= Quantidade;
        animator.Ataque();

        if (SaudeAtual <= 0)
        {
            Die();
        }
    }
    public bool Die()
    {
        return isDead;
    }
}
