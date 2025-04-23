using UnityEngine;

public class Platform : MonoBehaviour
{
    public float distance;
    public float velocity;
    public bool move = true;
    Vector3 position;
    public Vector3 direction = Vector3.forward;
    Rigidbody rdb;
    [SerializeField]
    float m;

    void Start()
    {
        position = transform.position;
        rdb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (move)
        {
            m = Mathf.Sin(Time.time * velocity) + 1;
            transform.position = position + direction.normalized * (m * distance / 2);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.parent = null;
        }
    }
}