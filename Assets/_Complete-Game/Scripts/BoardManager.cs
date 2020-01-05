using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count interiorWallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject[] exitTiles;
    public GameObject tree;
    public GameObject gift;
    public GameObject[] floorTiles;
    public GameObject[] interiorFloorTiles;
    public GameObject[] wallTiles;
    public GameObject[] interiorWallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] interiorOuterWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        } 
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range (0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range (0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }
    void InteriorBoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = interiorFloorTiles[Random.Range(0, interiorFloorTiles.Length)];
                if (x == -1 && y == -1)
                    toInstantiate = interiorOuterWallTiles[0];
                else if (x == columns && y == -1)
                    toInstantiate = interiorOuterWallTiles[2];
                else if (x == -1)
                    toInstantiate = interiorOuterWallTiles[3];
                else if (x == columns)
                    toInstantiate = interiorOuterWallTiles[4];
                else if (y == -1)
                    toInstantiate = interiorOuterWallTiles[1];
                else if (y == rows)
                    toInstantiate = interiorOuterWallTiles[Random.Range(5, interiorOuterWallTiles.Length)];
                else if (x < 2 || x > 5 || y < 2 || y > 4)
                    toInstantiate = interiorFloorTiles[Random.Range(12, interiorFloorTiles.Length)];
                else if (x == 2 && y == 2)
                    toInstantiate = interiorFloorTiles[0];
                else if (x == 2 && y == 3)
                    toInstantiate = interiorFloorTiles[1];
                else if (x == 2 && y == 4)
                    toInstantiate = interiorFloorTiles[2];
                else if (x == 3 && y == 2)
                    toInstantiate = interiorFloorTiles[3];
                else if (x == 3 && y == 3)
                    toInstantiate = interiorFloorTiles[4];
                else if (x == 3 && y == 4)
                    toInstantiate = interiorFloorTiles[5];
                else if (x == 4 && y == 2)
                    toInstantiate = interiorFloorTiles[6];
                else if (x == 4 && y == 3)
                    toInstantiate = interiorFloorTiles[7];
                else if (x == 4 && y == 4)
                    toInstantiate = interiorFloorTiles[8];
                else if (x == 5 && y == 2)
                    toInstantiate = interiorFloorTiles[9];
                else if (x == 5 && y == 3)
                    toInstantiate = interiorFloorTiles[10];
                else if (x == 5 && y == 4)
                    toInstantiate = interiorFloorTiles[11];
                
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }
    public void LeaveGift()
    {
        Instantiate(gift, new Vector3(columns - 1, rows - 2, 0F), Quaternion.identity);
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        GameObject toInstantiate = exitTiles[Random.Range(0, exitTiles.Length)];
        Vector3 randomPosition = RandomPosition();
        Instantiate(foodTiles[0], randomPosition, Quaternion.identity);
        Instantiate(toInstantiate, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
    public void SetupInteriorScene(int level)
    {
        InteriorBoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(interiorWallTiles, interiorWallCount.minimum, interiorWallCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(tree, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);
    }
}
