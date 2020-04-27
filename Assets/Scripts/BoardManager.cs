using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
	[Serializable] public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] cornerWallTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	void InitialiseList () {
		gridPositions.Clear();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				// Outer walls
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
					
					// Corners
					if ((x == -1 || x == columns) && (y == -1 || y == rows)) {
						toInstantiate = cornerWallTiles[Random.Range(0, cornerWallTiles.Length)];
					}

				}

				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
				
				instance.transform.SetParent(boardHolder);

				// Rotate left wall
				if (x == -1 && y != -1 && y != rows) {
					instance.transform.Rotate(0,0,90);
				}
				// Rotate right wall
				if (x == columns && y != -1 && y != rows) {
					instance.transform.Rotate(0,0,-90);
				}
				// Rotate bottom wall
				if (y == -1 && x != -1 && x != columns) {
					instance.transform.Rotate(0,0,180);
				}
				// Rotate corners
				if (x == -1 && y == rows) {
					instance.transform.Rotate(0,0,-90);
				}
				if (x == columns && y == rows) {
					instance.transform.Rotate(0,0,-180);
				}
				if (x == columns && y == -1) {
					instance.transform.Rotate(0,0,90);
				}
			}
		}
	}

    Vector3 RandomPosition () {
    	int randomIndex = Random.Range(0, gridPositions.Count);
    	Vector3 randomPosition = gridPositions[randomIndex];
    	gridPositions.RemoveAt(randomIndex);
    	return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
    	int objectCount = Random.Range(minimum, maximum + 1);

    	for (int i = 0; i < objectCount; i++) {
    		Vector3 randomPosition = RandomPosition();
    		GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
    		Instantiate(tileChoice, randomPosition, Quaternion.identity);
    	}
    }

    public void SetupScene (int level) {
    	BoardSetup();
    	InitialiseList();

    	LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
    	LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

    	int enemyCount = (int)Mathf.Log(level, 2f);
    	LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
    	Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
