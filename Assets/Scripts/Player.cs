using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public Text foodText;
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;
    public ParticleSystem footprint;
    public GameObject speechBubble;
    public Vector2 movement;
    public Vector2 lastPosition;

    private Animator animator;
	private int food;
	private Vector2 touchOrigin = -Vector2.one;
    private int colorCount = 0;
    private int foodWarningLimit = 50;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = food.ToString();

        base.Start();
    }

    private void OnDisable() {
    	GameManager.instance.playerFoodPoints = food;
    }


    void Update()
    {
        //print(Input.mousePosition);
        if (!GameManager.instance.playersTurn) {
        	return;
        }

        int horizontal = 0;
        int vertical = 0;

        if (foodText.color == Color.green) {
            colorCount++;

            // Show text for 60 frames
            if (colorCount > 60) {
                colorCount = 0;
                foodText.color = Color.black;
            }
        }
        else {
            if (food < foodWarningLimit) {
                foodText.color = Color.red;
            }
            else {
                foodText.color = Color.black;
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", (float) horizontal);
        animator.SetFloat("Vertical", (float) vertical);

        if (horizontal != 0) {
        	vertical = 0;
            CreateFootprint();
        }

#else  
     
        if (Input.touchCount > 0) {
        	Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began) {
            	touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x > 0) {
            	Vector2 touchEnd = myTouch.position;
            	float x = touchEnd.x - touchOrigin.x;
            	float y = touchEnd.y - touchOrigin.y;
            	touchOrigin.x = -1;

            	if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    if (x > 100)
                    {
                        horizontal = 1;
                    } else if(x < -100)
                        horizontal = -1;
            		
            	}
            	else{
                    if(y > 100)
                    {
                        vertical = 1;
                    }else if(y < -100)
                    {
                        vertical = -1;
                    }
            		
            	}
            }
        }

        animator.SetFloat("Horizontal", (float) horizontal);
        animator.SetFloat("Vertical", (float) vertical);
#endif

        if (horizontal != 0 || vertical != 0) {
        	AttemptMove <Wall> (horizontal, vertical);
            animator.SetFloat("Speed", 0.5f);
            CreateFootprint();
        }
        else {
            animator.SetFloat("Speed", 0f);
        }
    }


    protected override void AttemptMove <T> (int xDir, int yDir) {
    	food--;
        foodText.text = food.ToString();

        
        lastPosition = new Vector2(transform.position.x, transform.position.y);

        base.AttemptMove<T>(xDir, yDir);

    	RaycastHit2D hit;
    	if (Move(xDir, yDir, out hit)) {
    		SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
    	}

    	CheckIfGameOver();

    	GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other) {
    	if (other.tag == "Exit") {
    		Invoke ("Restart", restartLevelDelay);
    		enabled = false;
    	}
    	else if (other.tag == "Food") {
    		food += pointsPerFood;
        	foodText.text = food.ToString();
            foodText.color = Color.green;
    		SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
    		other.gameObject.SetActive(false);
    	}
    	else if (other.tag == "Soda") {
    		food += pointsPerSoda;
        	foodText.text = food.ToString();
            foodText.color = Color.green;
    		SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
    		other.gameObject.SetActive(false);
    	}
    }

    protected override void OnCantMove <T> (T component) {
    	Wall hitWall = component as Wall;
    	hitWall.DamageWall(wallDamage);
    	animator.SetTrigger("playerChop");
    }

    private void Restart() {
        Debug.Log("Gamemanager score " + GameManager.instance.score);
       // GameManager.instance.score = ScoreManager.instance.score;
        // GameManager.instance.highScore = ScoreManager.instance.highScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss, int dir) {
    	food -= loss;
        foodText.text = food.ToString();

        if(dir == 1)
        {
            animator.SetTrigger("TakeDamageRight");
        }else if(dir == 2)
        {
            animator.SetTrigger("TakeDamageLeft");
        }
    	CheckIfGameOver();
    }

    private void CheckIfGameOver() {
    	if (food <= 0) {
    		SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    void CreateFootprint()
    {
        footprint.Play();
    }
}
