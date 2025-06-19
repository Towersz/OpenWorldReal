using UnityEngine;

public class AtributoInimigo : MonoBehaviour
{
    public int InimigoHealth;
    public int InimigoAtaque;

    public void TakeDamage(int amount)
    {
        InimigoHealth -= amount;
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AtributoInimigo>();
        if (atm != null)
        {
            atm.TakeDamage(InimigoAtaque);
        }
    }

    private void Update()
    {
        if (InimigoHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
