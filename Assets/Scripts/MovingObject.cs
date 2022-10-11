using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	private float movementSpeed;

	public LayerMask blockingLayer;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rig;

	protected virtual void Awake(){
		boxCollider = GetComponent<BoxCollider2D> ();
		rig = GetComponent<Rigidbody2D> ();
	}


	// Use this for initialization
	protected virtual void Start () {
		movementSpeed = 1 / moveTime;
	}

	protected IEnumerator SmoothMovement(Vector2 end){
		float remainingDistance = Vector2.Distance (rig.position,end);
		while (remainingDistance > float.Epsilon) {
			Vector2 newPosition = Vector2.MoveTowards(rig.position,end,movementSpeed*Time.deltaTime);
			rig.MovePosition (newPosition);
			remainingDistance = Vector2.Distance (rig.position,end);
			yield return null;
		}
	}


	protected bool Move(int xDir,int yDir,out RaycastHit2D hit){
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			StartCoroutine (SmoothMovement (end));
			return true;
		}
		return false;
	}
		
	protected abstract void OnCantMove (GameObject go);

	protected virtual void OnCanMove(){
	}
	protected virtual void AttempMove(int xDir,int yDir){
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);
		if (canMove) {
			OnCanMove ();
		} else {
			OnCantMove (hit.transform.gameObject);
		}
	}
}
