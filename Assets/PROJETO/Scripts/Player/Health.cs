using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public bool isDead = false;

    public GameObject deathEffect;
    public GameObject respawnEffect;
    public GameObject healEffect;

    public Vector3 respawnPoint;
    public float respawnTime = 5f;

    public Animator animator;
    public CharacterController cc;

    [Header("Barra de Vida UI")]
    public Slider lifeSlider; 

    void Start()
    {
        currentHealth = maxHealth;
        respawnPoint = transform.position;

        cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            animator = cc.GetComponent<Animator>();
        }

        if (lifeSlider != null)
        {
            lifeSlider.maxValue = maxHealth;
            lifeSlider.value = currentHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnPoint"))
        {
            respawnPoint = other.transform.position;
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;

        if (lifeSlider != null)
            lifeSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        animator.SetTrigger("Heal");
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (lifeSlider != null)
            lifeSlider.value = currentHealth;
    }

    public void Die()
    {
        animator.SetBool("Die", true);
        isDead = true;
        gameObject.SetActive(false);
        Invoke("Respawn", respawnTime);
    }

    public void Respawn()
    {
        animator.SetBool("Die", false);
        isDead = false;
        currentHealth = maxHealth;

        if (lifeSlider != null)
            lifeSlider.value = currentHealth;

        transform.position = respawnPoint;
        gameObject.SetActive(true);
    }
}