using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
	public AudioClip atack1, atack2;

	public int playerDmg;
	public Animator anim;
	public Transform target;
	public bool skypMove;


	protected override void Awake(){
		anim = GetComponent<Animator> ();
		base.Awake ();
	}

	protected override void OnCantMove(GameObject go){
		Player hitPlayer = go.GetComponent<Player> ();
		if (hitPlayer != null) {
			hitPlayer.LoseFood (playerDmg);
			anim.SetTrigger ("EnemyAtack");
			SoundManager.instance.RandomSfx (atack1, atack2);
		}
	}
		
	protected override void Start () {
		GameManager.instance.AddEnemyToList (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
	
	protected override void AttempMove(int xDir,int yDir){
		if (skypMove) {
			skypMove = false;
			return;
		}
		base.AttempMove (xDir,yDir);
		skypMove = true;
	}

	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
			yDir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
		AttempMove (xDir, yDir);
	}	
}
