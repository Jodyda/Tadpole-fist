using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Path : object
{
    public int distA; // Steps from A to this
    public int distB; // Steps from this to B
    public Path parent;
    public int x;
    public int y;

    public Path (int _distA, int _distB, Path _parent, int _x, int _y)
    {
        distA = _distA;
        distB = _distB;
        parent = _parent;
        x = _x;
        y = _y;
    }
    public int score // Total score for this
    {
        get 
        {
            return distA+distB;
        }
    }
}

public class Enemy : MovingObject
{
	public int playerDamage;

	private Animator animator;
	private Transform target;
	//private bool skipMove;
	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;
    public BoardManager boardScript;

    private List<Path> availablePath;

    protected override void Start()
    {
    	GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        boardScript = GameManager.instance.GetComponent<BoardManager>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir) {
    	base.AttemptMove <T> (xDir, yDir);
    }

    public void MoveEnemy() {
        /*if (skipMove) {
            skipMove = false;
            return;
        }
        skipMove = true;*/

        // Find possible paths
        availablePath = FindPath();

        // Get next move from possible paths and remove it from list
        if (availablePath != null) {
            /*Debug.Log("Correct target: " + ((target.position.x == availablePath[availablePath.Count - 1].x) &&
                    (target.position.y == availablePath[availablePath.Count - 1].y)));
            Debug.Log("Player: X" + target.position.x + ", Y" +  target.position.y);
            Debug.Log("Target: X" + availablePath[availablePath.Count - 1].x + ", Y" +  availablePath[availablePath.Count - 1].y);*/

            Path nextMove = availablePath[0];
            availablePath.RemoveAt(0);

            int xDir = 0;
            int yDir = 0;

            if (nextMove.x == transform.position.x) {
                yDir = nextMove.y > transform.position.y ? 1 : -1;
            }
            else {
                xDir = nextMove.x > transform.position.x ? 1 : -1;
            }

        	AttemptMove<Player>(xDir, yDir);
        }
    }

    protected override void OnCantMove <T> (T component) {
    	Player hitPlayer = component as Player;

    	animator.SetTrigger("enemyAttack");

    	hitPlayer.LoseFood(playerDamage);

    	SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }

    // Path-finding for smarter enemies
    private List<Path> GetAdjacentSquares(Path p) {
        List<Path> paths = new List<Path>();

        int currX = p.x;
        int currY = p.y;

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                int newX = currX + x;
                int newY = currY + y;

                // Skip self and diagonals
                if ((x == 0 && y == 0) || (x != 0 && y != 0)) {
                    continue;
                }
                else if (newX < boardScript.columns && newX >= 0
                    && newY < boardScript.rows && newY >= 0) {
                    // New square is within the game board

                    // Check if new square is available for move
                    if (!CheckForCollision(new Vector2(currX,currY),new Vector2(newX,newY))) {
                        // There is nothing in the way on new square
                        paths.Add(new Path(p.distA+1, BlocksToTarget(new Vector2(newX,newY), target.position), p, newX, newY));
                    }
                }
            }
        }

        return paths;
    }

    // Check if direction is blocked
    private bool CheckForCollision(Vector2 start, Vector2 end) {
        // Disable box collider
        this.GetComponent<BoxCollider2D>().enabled = false;

        // Cast line and check for hit
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);

        // Reenable box collider
        this.GetComponent<BoxCollider2D>().enabled = true;

        // Did it hit something other than the Player in the blocking layer?
        if (hit.transform != null && !hit.collider.tag.Equals("Player")) {
            return true;
        }

        return false;
    }

    // Estimate how far the path is from target
    private int BlocksToTarget(Vector2 start, Vector2 end)
    {
        float floatDistX = Mathf.Abs(end.x - start.x);
        int distX = Mathf.RoundToInt(floatDistX);

        float floatDistY = Mathf.Abs(end.y - start.y);
        int distY = Mathf.RoundToInt(floatDistY);

        return distX + distY;
    }

    // Reverse list of paths
    private List<Path> BuildPath(Path p) {
        List<Path> path = new List<Path>();
        Path currentLoc = p;
        path.Insert(0,currentLoc);

        while (currentLoc.parent != null) {
            currentLoc = currentLoc.parent;

            if (currentLoc.parent != null) {
                path.Insert(0, currentLoc);
            }
        }

        return path;
    }

    private Path GetBestScoringItem(List<Path> paths) {
        // Start with first item as reference
        Path bestScoringItem = paths[0];

        // If there are more items, compare their score
        if (paths.Count > 1) {
            for (int i = 1; i < paths.Count; i++) {
                if (bestScoringItem.score > paths[i].score) {
                    bestScoringItem = paths[i];
                }
            }
        }

        return bestScoringItem;
    }

    private bool DoesPathListContain(List<Path> paths, Path path) {
        // Compare paths in list to target path
        for (int i = 0; i < paths.Count; i++) {
            if (paths[i].x == path.x && paths[i].y == path.y) {
                return true;
            }
        }

        return false;
    }

    private List<Path> FindPath() {
        List<Path> evaluationList = new List<Path>();
        List<Path> evaluatedSquares = new List<Path>();
        Vector3 enemyPos = transform.position;

        // Target position
        Path destinationSquare = new Path(BlocksToTarget(enemyPos, target.position), 0, null, (int)target.position.x, (int)target.position.y);

        // Enemy position
        evaluationList.Add(new Path(0, BlocksToTarget(enemyPos, target.position), null, (int)enemyPos.x, (int)enemyPos.y));

        // Loop through evaluation list
        Path currentSquare = null;
        while (evaluationList.Count > 0) {
            currentSquare = GetBestScoringItem(evaluationList);

            evaluatedSquares.Add(currentSquare);
            evaluationList.Remove(currentSquare);

            if (DoesPathListContain(evaluatedSquares, destinationSquare)) {
                // We found the target, return a reversed path to it
                return BuildPath(currentSquare);
            }

            // Loop though adjacent squares
            List<Path> adjacentSquares = GetAdjacentSquares(currentSquare);
            foreach (Path square in adjacentSquares) {
                if (DoesPathListContain(evaluatedSquares, square)) {
                    // This has already been checked
                    continue;
                }

                if (!DoesPathListContain(evaluationList, square)) {
                    // Add path for evaluation
                    evaluationList.Add(square);
                }
                else if (square.distB + currentSquare.distA + 1 < square.score) {
                    // Square scores better with current path as parent, switch in list
                    square.parent = currentSquare;
                }
            }
        }

        return null;
    }
}


