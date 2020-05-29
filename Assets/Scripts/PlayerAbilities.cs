using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject TadpoleDrop;

   
    private Player player;
    public TadpoleSlider tadpoleSlider;
    public int maxTadpole = 4;
    public int currentTadpole;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentTadpole = maxTadpole;
        tadpoleSlider.SetMaxCount(maxTadpole);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        { 
            DropItem();
        }
        
    }


    void DropItem()
    {
    

        if (TadpoleDrop && currentTadpole > 0)
        {
            Instantiate(TadpoleDrop, player.lastPosition,
            TadpoleDrop.transform.rotation);
            currentTadpole--;

            tadpoleSlider.SetCount(currentTadpole);
            
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tadpole")
        {
            Debug.Log("Collision!!");
            Destroy(collision.gameObject);

            currentTadpole++;
            tadpoleSlider.SetCount(currentTadpole);
        }
    }


}






