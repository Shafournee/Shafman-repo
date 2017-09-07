using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    GameObject Player;
    GameObject GameManager;
    public Text FloorLevel;
    int FloorLevelNumber;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        FloorLevelNumber = GameManager.GetComponent<GameManager>().FloorLevel;
        FloorLevel.text = FloorLevelNumber.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MenuButton()
    {
        SceneManager.LoadScene("TitleScreen");
        Destroy(Player);
        Destroy(GameManager);
    }

}
