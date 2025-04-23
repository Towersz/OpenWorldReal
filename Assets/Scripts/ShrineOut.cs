using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShrineOut : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
            // Vector3 returnPosition = PlayerPrefsX.GetVector3("OldPlayerPosition", other.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            
            SceneManager.LoadScene("terrain cena");
            
            

        }
    }
}
