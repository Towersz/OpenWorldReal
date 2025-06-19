using UnityEngine;

public class Damegetester : MonoBehaviour
{
    public AtributoPlayer AtmPlayer;
    public AtributoInimigo AtmInimigo;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AtmPlayer.DealDamage(AtmInimigo.gameObject);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            AtmInimigo.DealDamage(AtmPlayer.gameObject);
        }
    }
}
