using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public int columns = 8;
	public int rows = 8;
	public GameObject[] floorTiles, outerWallTiles,wallTiles,foodTikles,EnemyTiles;
	public GameObject exit;
	private Transform BoardHolder;
	private List<Vector2> gridPos = new List<Vector2>();

	void initializeList(){
		gridPos.Clear ();
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPos.Add(new Vector2(x,y));
			}
		}
	}

	Vector2 RandomPos(){
		int randomeIndex = Random.Range (0, gridPos.Count);
		Vector2 randomPos = gridPos [randomeIndex];
		gridPos.RemoveAt (randomeIndex);
		return randomPos;
	}

	void layoutObjectAtRandom(GameObject[] tileArray,int min,int max){
		int objectCount = Random.Range (min, max + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector2 randomPos = RandomPos ();
			GameObject tileChoice = GetRandomeInArray (tileArray);
			Instantiate (tileChoice, randomPos, Quaternion.identity);
		}
	}


	public void SetupScene(int level){
		BoardSetup ();
		initializeList ();
		layoutObjectAtRandom (wallTiles,5,9);
		layoutObjectAtRandom (foodTikles,1,6);
		int enemycount = (int)Mathf.Log(level,2);
		layoutObjectAtRandom (EnemyTiles,enemycount,enemycount);
		Instantiate (exit, new Vector2 (columns - 1, rows - 1), Quaternion.identity);
	}

	public void BoardSetup(){
		BoardHolder = new GameObject ("Board").transform;
		for(int x=-1;x<columns+1;x++){
			for(int y=-1;y<rows+1;y++){
				GameObject Toinstantiate = GetRandomeInArray(floorTiles);
				if (x == -1 || y == -1 || x == columns || y == rows) {
					Toinstantiate = GetRandomeInArray(outerWallTiles);
				}

				GameObject instance = Instantiate (Toinstantiate, new Vector2 (x, y), Quaternion.identity);
				instance.transform.SetParent (BoardHolder);
			}
		}
	}

	GameObject GetRandomeInArray(GameObject[] array){
		return array [Random.Range (0, array.Length)];
	}


}
