using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleManager : MonoBehaviour
{
  
    
  

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //collision.gameObject.layer = 0;
            Debug.Log("Collision");
            this.gameObject.SetActive(false);
        }
    }
    
    

}
