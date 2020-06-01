using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject soundManager;
	public float levelStartDelay = 2f;
    public float speechBubbleDelay = 2f;
    public float gameOverDelay = 2f;
	public BoardManager boardScript;
    public PhraseDecoder phraseDecoder;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

    public static float turnDelay = 0.3f;
    public static GameManager instance = null;

	private Text levelText;
	private GameObject levelImage;
	private int level = 0;
	private List<Enemy> enemies;
    private Player player;
	private bool enemiesMoving;
	private bool doingSetup;

    public int score;
    public int highScore;


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
        phraseDecoder = GetComponent<PhraseDecoder>();
	}

    private void Start()
    {
        ScoreManager.instance.Load();
    }

    // This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
        // ScoreManager.instance.score = score;
       // ScoreManager.instance.highScore = highScore;
        ScoreManager.instance.AddScore(); 
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
        
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void HideLevelImage() {
    	levelImage.SetActive(false);
    	doingSetup = false;

        if (level % 2 == 0 && enemies.Count != 0) {
            ShowSpeechBubbles();
        }
    }

    private void ShowSpeechBubbles() {
        Phrase phrase = phraseDecoder.GetRandomPhrase();

        Enemy enemy = enemies[Random.Range(0, enemies.Count)];
        enemy.speechBubble.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(phrase.enemy);
        enemy.speechBubble.SetActive(true);

        StartCoroutine(HideEnemyBubble(enemy));
        StartCoroutine(ShowPlayerBubble(phrase.player));
    }

    IEnumerator HideEnemyBubble(Enemy enemy) {
        yield return new WaitForSeconds(speechBubbleDelay);
        enemy.speechBubble.SetActive(false);
    }

    IEnumerator ShowPlayerBubble(string phrase) {
        yield return new WaitForSeconds(speechBubbleDelay);
        player.speechBubble.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(phrase);
        player.speechBubble.SetActive(true);

        StartCoroutine(HidePlayerBubble());
    }

    IEnumerator HidePlayerBubble() {
        yield return new WaitForSeconds(speechBubbleDelay);
        player.speechBubble.SetActive(false);
    }

    public void GameOver() {
        levelText.text = "After " + level + " days, you starved";
        levelImage.SetActive(true);

        Invoke("EndGame", gameOverDelay);
    }

    public void EndGame() {
        enabled = false;
        Destroy(gameObject);
        ScoreManager.instance.Save();
        SceneManager.LoadScene(0);
        SoundManager.instance.musicSource.Play();
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
    		yield return new WaitForSeconds(turnDelay);
    	}

    	playersTurn = true;
    	enemiesMoving = false;
    }
}
