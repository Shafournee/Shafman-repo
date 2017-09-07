using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject Player;
    public int FloorLevel;
    //For Item Generation
    public List<GameObject> ItemList;
    public GameObject FinalItem;
    GameObject RandomlyGeneratedItem;
    //For boss generation
    public List<GameObject> BossList;
    public GameObject FinalBoss;
    GameObject RandomlyGeneratedBoss;
    GameObject Canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        FloorLevel = 1;
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //Handles pausing and unpausing the game
    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Canvas.GetComponent<PlayerUI>().EnableThePauseMenu();
        }
        else
        {
            Canvas.GetComponent<PlayerUI>().DisableThePauseMenu();
        }
    }

    //Handles removing generated items from the item list
    public GameObject GenerateAGameObject()
    {
        if (ItemList.Count == 0)
        {
            return FinalItem;
        }
        else
        {
            //Pull a random room from the array
            RandomlyGeneratedItem = ItemList[Random.Range(0, ItemList.Count - 1)];
            ItemList.Remove(RandomlyGeneratedItem);
            return RandomlyGeneratedItem;
        }
    }
    //Handles removing bosses from the pool and ramdomly generating them
    public GameObject GenerateABoss()
    {
        if (BossList.Count == 0)
        {
            return FinalBoss;
        }
        else
        {
            RandomlyGeneratedBoss = BossList[Random.Range(0, BossList.Count - 1)];
            BossList.Remove(RandomlyGeneratedBoss);
            return RandomlyGeneratedBoss;
        }
    }

}
