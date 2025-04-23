using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float distance;
    public float velocity;
    public bool move = true;
    Vector3 position;
    Rigidbody rdb;
    [SerializeField]
    float m;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        rdb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            m = Mathf.Sin(Time.time * velocity) +1* distance;

            transform.position = position +
                new Vector3(0, 0, 0 );
        }


    }
    private void OnTriggerEnter(Collider col)
    {
      
        if (col.CompareTag("Player"))
        {
           
            col.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.transform.parent = null;
        }
    }


}
