using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1, drinkSound2,gameoverSound;

	public int 	wallDamage = 1;
	public int pointPerFood = 10;
	public int pointPerSoda = 20;
	public float restarlevelDelay = 1f;
	public Text foodText;

	private Animator anim;
	private int food;

	protected override void Awake(){
		anim = GetComponent<Animator> ();
		base.Awake ();
	}

	// Use this for initialization
	protected override void Start () {
		food = GameManager.instance.playerFootPoints;
		foodText.text = "Food " + food;
		base.Start ();
	}

	private void OnDisable(){
		GameManager.instance.playerFootPoints = food;
	}

	void CheckIfGameover(){
		if (food <= 0) {
			SoundManager.instance.PlaySingle (gameoverSound);
			SoundManager.instance.musicSound.Stop ();
			GameManager.instance.GameOver ();

		}
	}

	protected override void AttempMove(int xDir,int yDir){
		food--;
		foodText.text = "Food " + food;
		base.AttempMove (xDir, yDir);

		CheckIfGameover ();
		GameManager.instance.playerTurn = false;
	}


	protected override void OnCantMove (GameObject go){
		Wall hitwall = go.GetComponent<Wall> ();
		if (hitwall != null) {
			hitwall.DamageWall (wallDamage);
			anim.SetTrigger ("PlayerShot");
		}
	}

	void Update(){
		if (!GameManager.instance.playerTurn || GameManager.instance.doingSetup)
			return;
		int horizontal;
		int vertical;
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;
		if (horizontal != 0 || vertical != 0) AttempMove (horizontal, vertical);
	}

	void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void LoseFood(int loss){
		food -= loss;
		foodText.text = "-"+ loss + " Food " + food;
		anim.SetTrigger ("PlayerHit");
		CheckIfGameover ();
	}

	public void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Exit")){
			Invoke("Restart",restarlevelDelay);
			enabled = false;
		} else if(other.CompareTag("Food")){
			food += pointPerFood;
			SoundManager.instance.RandomSfx (eatSound1,eatSound2);
			foodText.text = "+"+ pointPerFood + " Food " + food;
			other.gameObject.SetActive (false);
		} else if(other.CompareTag("Soda")){
			food += pointPerSoda;
			SoundManager.instance.RandomSfx (drinkSound1,drinkSound2);
			foodText.text = "+"+ pointPerSoda + " Food " + food;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCanMove ()
	{
		base.OnCanMove ();
		SoundManager.instance.RandomSfx (moveSound1, moveSound2);
	}
}
