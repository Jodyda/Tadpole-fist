using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject soundManager;
	public float levelStartDelay = 2f;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

    public static float turnDelay = 0.2f;
    public static GameManager instance = null;

	private Text levelText;
	private GameObject levelImage;
	private int level = 0;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;


    void Awake ()
    {
    	if (instance == null) {
    		instance = this;
    	}
    	else if (instance != this) {
    		Destroy(gameObject);
    	}

    	DontDestroyOnLoad(gameObject);
    	
    	enemies = new List<Enemy>();
        soundManager = GameObject.Find("SoundManager");
        boardScript = GetComponent<BoardManager>();
	}

    // This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		level++;
		InitGame();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

    void InitGame () {
    	doingSetup = true;

    	levelImage = GameObject.Find("LevelImage");
    	levelText = GameObject.Find("LevelText").GetComponent<Text>();
    	levelText.text = "Room " + level;
    	levelImage.SetActive(true);
    	Invoke("HideLevelImage", levelStartDelay);

    	enemies.Clear();
    	boardScript.SetupScene(level);
    }

    private void HideLevelImage() {
    	levelImage.SetActive(false);
    	doingSetup = false;
    }

    public void GameOver() {
    	levelText.text = "After " + level + " days, you starved";
        Destroy(soundManager);
        Destroy(gameObject);
        levelImage.SetActive(true);
    	enabled = false;
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update ()
    {
        if (playersTurn || enemiesMoving || doingSetup) {
        	return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script) {
    	enemies.Add(script);
    }

    IEnumerator MoveEnemies() {
    	enemiesMoving = true;
    	yield return new WaitForSeconds(turnDelay);

    	if (enemies.Count == 0) {
    		yield return new WaitForSeconds(turnDelay);
    	}

    	for (int i = 0; i < enemies.Count; i++) {
    		enemies[i].MoveEnemy();
    		yield return new WaitForSeconds(enemies[i].moveTime);
    	}

    	playersTurn = true;
    	enemiesMoving = false;
    }
}
