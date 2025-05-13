using UnityEngine;
using UnityEngine.AI;

public class IACorre : MonoBehaviour
{
   public NavMeshAgent agent ;  
   public GameObject chaser ;
    [SerializeField] private float disPos;
    
    void Start()
    {
        if(!chaser)
        {
           chaser = GameObject.FindGameObjectWithTag("Player");
        }
        agent.destination = RandomPosition(100);
    }

    // Update is called once per frame
    private void Update()
    {
       
         
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("cahsing");
            agent.speed = 10;
            Vector3 normDir = (chaser.transform.position - transform.position).normalized;
            normDir = Quaternion.AngleAxis(Random.Range(0, 170), Vector3.up) * normDir;
        }
    }
    void OnTriggerExit(Collider other)
    {
        agent.speed = 3;
        agent.destination = RandomPosition(100);
    }



    Vector3 RandomPosition(float range)
    {
        Vector3 pos;
        pos = transform.position + new Vector3(UnityEngine.Random.Range(-range, range)
            , 0
            , UnityEngine.Random.Range(-range, range));
        return pos;
    }

}
