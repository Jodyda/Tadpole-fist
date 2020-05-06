using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject TadpoleDrop;

    public int TadpoleCount = 4;


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DropItem();
        }
    }

    void DropItem()
    {
        if (TadpoleDrop && TadpoleCount > 0)
        {
            Instantiate(TadpoleDrop, new Vector2(Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)),
            TadpoleDrop.transform.rotation);
            TadpoleCount--;
        }

    }
       
    
}




//Utbytt kod
//Instantiate(TadpoleDrop, this.gameObject.transform.position - transform.forward * 4, Quaternion.identity);


// Instantiate(TadpoleDrop, this.gameObject.transform.position - transform.forward * 4, Quaternion.identity);
