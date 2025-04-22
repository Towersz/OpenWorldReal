using UnityEngine;

public class Qubrando : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
