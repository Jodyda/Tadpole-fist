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
        if (!PauseMenu.GameIsPaused)
        {
            DropController();

        }
    }

    void DropController()
    {
        if (IsDoubleTap())
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


    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }

}






