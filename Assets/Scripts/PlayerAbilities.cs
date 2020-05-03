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
        Instantiate(TadpoleDrop, this.gameObject.transform.position - transform.forward * 4, Quaternion.identity);
    }
}




// 