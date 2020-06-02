using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{

    [SerializeField]
    private GameObject TadpoleDrop;



    private Player player;


    //public float x = Input.mousePosition.x;
    //public float y = Input.mousePosition.y;


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
        if (!PauseMenu.GameIsPaused && Input.mousePosition.x > 200 && Input.mousePosition.x < 1300 && Input.mousePosition.y > 0 && Input.mousePosition.y < 600)
        {
            DropController();

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

    void DropController()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
        {
            DropItem();
        }
    }


}






