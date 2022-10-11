using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
	public AudioClip shop1, shop2;

	public Sprite dmgSprite;
	public int HP = 4;
	private SpriteRenderer spriteRander;

	private void Awake () {
		spriteRander = GetComponent<SpriteRenderer> ();
	}
	
	public void DamageWall(int loss){
		SoundManager.instance.RandomSfx (shop1, shop2);
		spriteRander.sprite = dmgSprite;
		HP -= loss;
		if (HP <= 0) {
			gameObject.SetActive (false);
		}
	}
}
