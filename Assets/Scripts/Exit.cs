using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

    GameObject Player;
    GameObject GameManager;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D Collider)
    {
        if(Collider.tag == "Player")
        {
            SceneManager.LoadScene("RandomGenerationTest");
            //Place the player in the origin room once generated
            Player.transform.position = new Vector3(0f, 0f, 1f);
            GameManager.GetComponent<GameManager>().FloorLevel++;
        }
    }
}
