using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

    protected override void Start()
    {
    	GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir) {
    	if (skipMove) {
    		skipMove = false;
    		return;
    	}

    	base.AttemptMove <T> (xDir, yDir);
    	skipMove = true;
    }

    public void MoveEnemy() {
    	int xDir = 0;
    	int yDir = 0;

    	if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
    		yDir = target.position.y > transform.position.y ? 1 : -1;
    	}
    	else {
    		xDir = target.position.x > transform.position.x ? 1 : -1;
    	}

    	AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove <T> (T component) {
    	Player hitPlayer = component as Player;

    	animator.SetTrigger("enemyAttack");
        int dir;
        /*if (target.position.x == transform.position.x)
        {
            dir = 0;
           if(target.position.y > transform.position.y)
            {
                dir = 2;
            }
        }else if (target.position.y == transform.position.y)
        {*/
            dir = 1;
        if (target.position.y == transform.position.y && target.position.x > transform.position.x)
            {
                dir = 2;
            
            }
        //}

    	hitPlayer.LoseFood(playerDamage, dir);

    	SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}


