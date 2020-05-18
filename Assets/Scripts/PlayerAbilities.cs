using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject TadpoleDrop;

    private Player player;
    
    

    public int TadpoleCount = 4;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

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
            Instantiate(TadpoleDrop, player.lastPosition,
            TadpoleDrop.transform.rotation);
            TadpoleCount--;
        }

    }
       
    
}






