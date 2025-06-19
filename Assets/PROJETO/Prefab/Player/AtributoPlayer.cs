using UnityEngine;
using UnityEngine.SceneManagement; // Adicione essa linha

public class AtributoPlayer : MonoBehaviour
{
    public int PlayerHealth;
    public int PlayerAtaque;

    public void TakeDamage(int amount)
    {
        PlayerHealth -= amount;
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AtributoPlayer>();
        if (atm != null)
        {
            atm.TakeDamage(PlayerAtaque);
        }
    }

    private void Update()
    {
        if (PlayerHealth <= 0)
        {
            Morte();
        }
    }

    private void Morte()
    {
        // Aqui você pode colocar o nome da sua cena de morte
        SceneManager.LoadScene("Morte");
    }
}
