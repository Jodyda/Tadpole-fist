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

	[Serializable] public class Environment {
		public GameObject exit;
		public GameObject entrance;
		public GameObject[] floorTiles;
		public GameObject[] wallTiles;
		public GameObject[] outerWallTiles;
		public GameObject[] cornerWallTiles;
	}

	public float columns = 8;
	public float rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject entrance;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] cornerWallTiles;

	public Environment indoor = new Environment();
	public Environment outdoor = new Environment();

	private List <Environment> environments = new List<Environment>();
	private Environment environment;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	void InitialiseList () {
		gridPositions.Clear();

		for (float x = 1; x < columns - 1; x++) {
			for (float y = 1; y < rows - 1; y++) {
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;

		for (float x = -1; x < columns + 1; x++) {
			for (float y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = environment.floorTiles[Random.Range(0, environment.floorTiles.Length)];

				// Outer walls
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = environment.outerWallTiles[Random.Range(0, environment.outerWallTiles.Length)];

					// Place floor tile, exit goes here
					if (y == rows && x == columns - 1) {
						toInstantiate = environment.floorTiles[Random.Range(0, environment.floorTiles.Length)];
					}

					// Place entrance tile
					if (y == 0 && x == -1) {
						toInstantiate = environment.entrance;
					}
					
					// Corners
					if ((x == -1 || x == columns) && (y == -1 || y == rows)) {
						toInstantiate = environment.cornerWallTiles[Random.Range(0, environment.cornerWallTiles.Length)];
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

    	Vector3[] positions = new Vector3[objectCount];

    	for (int i = 0; i < objectCount; i++) {
    		positions[i] = RandomPosition();
    	}

    	// We want to print items from bottom to top, so positions need to be sorted by Y value
    	Array.Sort(positions, delegate(Vector3 v1, Vector3 v2) {
			if( v2.y > v1.y ) {
				return 1;
			}
			else if( v2.y < v1.y ) {
				return -1;
			}
			return 0;
		});

    	for (int i = 0; i < objectCount; i++) {
    		GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
    		Instantiate(tileChoice, positions[i], Quaternion.identity);
    	}
    }

    Environment RandomEnvironment() {
    	int randomIndex = Random.Range(0, environments.Count);
    	return environments[randomIndex];
    }

    public void SetupScene (int level) {
    	environments.Add(indoor);
    	environments.Add(outdoor);

		environment = RandomEnvironment();

    	BoardSetup();
    	InitialiseList();

    	LayoutObjectAtRandom(environment.wallTiles, wallCount.minimum, wallCount.maximum);
    	LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

    	int enemyCount = (int)Mathf.Log(level, 2f);
    	LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
    	Instantiate(exit, new Vector3(columns - 1.5f, rows - 0.5f, 0f), Quaternion.identity);
    }
}
