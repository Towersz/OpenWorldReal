using UnityEngine;

public class InimigoAnimator : MonoBehaviour
{
    public Animator animator;
    private InimigoSaude saude;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        saude = GetComponent<InimigoSaude>();
    }
    public void UpdateSpeed(float velocidade)
    {
        animator.SetFloat("Speed", velocidade);
    }
    public void Ataque()
    {
        animator.SetTrigger("Ataque");
    }
    public void Morte()
    {
        animator.SetTrigger("Morte");
    }
}
