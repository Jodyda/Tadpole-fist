﻿using System.Collections;
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
    public AudioClip startNewLevel;
    public ParticleSystem footprint;


    Vector2 movement;
    public Vector2 lastPosition;

    private Animator animator;
	private int food;
	private Vector2 touchOrigin = -Vector2.one;



    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Energy: " + food;

        SoundManager.instance.RandomizeSfx(startNewLevel);

        base.Start();
    }

    private void OnDisable() {
    	GameManager.instance.playerFoodPoints = food;
    }


    void Update()
    {
        if (!GameManager.instance.playersTurn) {
        	return;
        }

        int horizontal = 0;
        int vertical = 0;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
  

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
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
            	Vector2 touchEnd = myTouch.position;
            	float x = touchEnd.x - touchOrigin.x;
            	float y = touchEnd.y - touchOrigin.y;
            	touchOrigin.x = -1;

            	if (Mathf.Abs(x) > Mathf.Abs(y)) {
            		horizontal = x > 0 ? 1 : -1;
            	}
            	else {
            		vertical = y > 0 ? 1 : -1;
            	}
            }
        }

#endif

        if (horizontal != 0 || vertical != 0) {
        	AttemptMove <Wall> (horizontal, vertical);
            CreateFootprint();
        }
    }

    //private void FixedUpdate()
    //{
     //   rb.MovePosition(rb.position * moveSpeed * Time.fixedDeltaTime);
    //}

    protected override void AttemptMove <T> (int xDir, int yDir) {
    	food--;
        foodText.text = "Energy: " + food;

        
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
        	foodText.text = "+" + pointsPerFood + " Energy: " + food;
    		SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
    		other.gameObject.SetActive(false);
    	}
    	else if (other.tag == "Soda") {
    		food += pointsPerSoda;
        	foodText.text = "+" + pointsPerSoda + " Energy: " + food;
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
        foodText.text = "-" + loss + " Food: " + food;

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
