using UnityEngine;

public class InimigoAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Ataque()
    {
        animator.SetTrigger("Ataque");
    }
    public void Golpe()
    {
        animator.SetTrigger("Golpe");
    }
    public void Morte()
    {
        animator.SetTrigger("Morte");
    }
}
