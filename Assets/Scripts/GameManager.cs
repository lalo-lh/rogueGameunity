using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float turnDelay=0.1f;
	public static GameManager instance;
	public BoardManager boardScript;
	public int playerFootPoints = 100;
	[HideInInspector]
	public bool playerTurn = true;

	private List<Enemy> enemies = new List<Enemy> ();
	private bool enemiesMoving;


	private int level=0;
	public float leveStart=2f;
	private GameObject levelImage;
	private Text levelText;
	public bool doingSetup;

	void Awake(){
		if (GameManager.instance == null) {
			GameManager.instance = this;
		} else if(GameManager.instance!=this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
	}

	void initGame(){
		doingSetup = true;
		levelImage = GameObject.Find ("Levelimage");
		levelText = GameObject.Find ("levelText").GetComponent<Text>();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		enemies.Clear ();
		boardScript.SetupScene (level);
		Invoke ("HideLevelImage", leveStart);	
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver(){
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		enabled = false;
	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}
		playerTurn = true;
		enemiesMoving = false;
	}

	private void Update(){
		if (!playerTurn && !enemiesMoving && !doingSetup) {
			StartCoroutine (MoveEnemies ());
		}
	}

	public void AddEnemyToList(Enemy enemy){
		enemies.Add (enemy);
	}

	private void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene,LoadSceneMode mode){
		level++;
		initGame ();
	}
}
