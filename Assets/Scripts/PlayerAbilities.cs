using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject TadpoleDrop;

   
    private Player player;


    public int maxTadpole = 4;
    public int currentTadpole;

    public Text tadpoleText;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentTadpole = maxTadpole;
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
            tadpoleText.text = currentTadpole.ToString();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tadpole")
        {
            Debug.Log("Collision!!");
            Destroy(collision.gameObject);

            currentTadpole++;
            tadpoleText.text = currentTadpole.ToString();
        }
    }


}






