using UnityEngine;
using System.Collections;
using System.Collections.Generic;        //Allows us to use Lists. 
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.
    private BoardManager boardScript;                        //Store a reference to our BoardManager which will set up the level.
    public float playerTime = 60;
    public int playerGifts = 0;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;                                    //Current level number, for 0 you didn't delivered any gift.
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    int interior;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        interior = 0;
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {

        InitGame();
    }
    //Initializes the game for each level.
    void InitGame()
    {
        doingSetup = true;


        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        enemies.Clear();
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        if (interior == 1)
        {
            levelText.text = "Now you have " + playerGifts + " gifts. Leave one on the tree.";
            levelImage.SetActive(true);
            Invoke("HideLevelImage", levelStartDelay);
            level++;
            boardScript.SetupInteriorScene(level);
            interior = 0;
        }
        else
        {
            if (level == 0)
            {
                levelStartDelay = 5f;
                levelText.text = "Hi Santa! You've lost some gifts along the way. \n Now you have to pick them up and leave one in each Christmas tree. \n Be careful with children, they will waste your time. \n If you collect the reindeer your sled will go faster \n and you will have more time to distribute gifts.";
                levelImage.SetActive(true);
                Invoke("HideLevelImage", levelStartDelay);
                boardScript.SetupScene(level);
                interior = 1;
            }
                
            else
            {
                levelStartDelay = 2f;
                levelText.text = "Great! You have delivered " + level + " gifts.";
                levelImage.SetActive(true);
                Invoke("HideLevelImage", levelStartDelay);
                boardScript.SetupScene(level);
                interior = 1;
            }
        }
    }
    public void LeaveGift()
    {
        boardScript.LeaveGift();
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = (false);
    }
    public void OnDificultyClick()
    {

    }
    public void GameOver()
    {
        enabled = false;
        levelText.text = "You made " + level + " kids happy.";
        levelImage.SetActive(true);
    }



    //Update is called every frame.
    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add (script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}