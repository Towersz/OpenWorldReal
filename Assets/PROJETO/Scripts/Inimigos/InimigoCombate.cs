using UnityEngine;

public class InimigoCombate : MonoBehaviour
{
    [SerializeField]
    private Collider Hitbox;
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float Cooldown = 2f;

    private bool HitboxAtivada = false;
    private float Ultimoataque = 0f;

    private InimigoAnimator animator;

    private void Start()
    {
        animator = GetComponent<InimigoAnimator>();
        if (animator != null ) 
            Hitbox.enabled = false;
    }

    public void TryAtaque(GameObject target)
    {
        if (Time.time >= Ultimoataque)
        {
            Ultimoataque = Time.time + Cooldown;
            animator.Ataque();
        }
    }
    public void Update()
    {
        //AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        //if (state.IsName("Ataque"))
        //{
        //    float t = state.normalizedTime % 1;
        //    if (t > 0.3f && t < 0.6f)
        //    {
        //        if (!HitboxAtivada)
        //        {
        //            Hitbox.enabled = true;
        //            HitboxAtivada = true;
        //        }
        //    }
        //    else
        //    {
        //        if (HitboxAtivada)
        //        {
        //            Hitbox.enabled = false;
        //            HitboxAtivada = false;
        //        }
        //    }
        //}
        //else
        //{
        //    Hitbox.enabled = false;
        //    HitboxAtivada = false;
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HitboxAtivada && other.CompareTag("Player"))
        {
            Debug.Log("Acertou");
        }
    }
}
