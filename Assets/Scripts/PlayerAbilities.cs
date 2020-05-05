using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject TadpoleDrop;


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DropItem();
        }
    }

    void DropItem()
    {
        if (TadpoleDrop)
        {
            Instantiate(TadpoleDrop, new Vector2(Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y)),
            TadpoleDrop.transform.rotation);
        }

    }
       
    
}




// Instantiate(TadpoleDrop, this.gameObject.transform.position - transform.forward * 4, Quaternion.identity);