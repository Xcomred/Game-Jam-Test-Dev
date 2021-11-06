using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.
        public count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
        // Why do we need this?? What is this for?
        // ^ Think of this of making a public call with just 1 constructor.
        // We will creat several objects that send data back to that object.
    }
    // this creates the width and height of our box we could make it larger or smaller
    public int columns = 8;
    public int rows = 8;
    
    public count wallCount = new count(5,8);
    public count foodCount = new count(1,5);// So these our creating new public object in the code. With the min and max set
    // What we are doing now is establishing a blue print to randomly creat our game state with said blue prints
    public GameObject exit;
    // We can leave it like this but we need a way to have more than one
    public GameObject[] floorTile; // So we make it an array
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] foodTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    //
    private List<Vector3> gridPositions = new List<Vector3>();

    // So we have our Blue print now we need a way to remove said blue print
    void InitialiseList()
    {
        // yeet yeet delete 
        gridPositions.Clear();
        // we use X as we our going through the X axis
        for(int x = 0; x < columns -1; x++) // why minus 1?
        {
            for(int y = 0; y < rows -1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

    }

    void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        //Loop through starting with -1
        for(int x = -1; x <columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                // so we have our loops now we need to get the tiles
                GameObject toInstantiate = floorTile[Random.Range(0, floorTile.Length)];
                // We need to check if the current postion is on the Edge then we want our wall tile 
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);

            }
        }
    }

    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        // now we want to creat objects until we hit a limit
        for (int i = 0; i < objectCount; i++)
        {
            // We are going to choose a random vector 3 from our aviable vector 3's 
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }
    }

    // we need a way to create a new scene after we clear and move to the next one.
    public void SetupScene (int level)
    {
        BoardSetUp();

        InitialiseList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

       
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);

        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
