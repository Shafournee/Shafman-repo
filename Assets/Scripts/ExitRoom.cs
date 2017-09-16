using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour {

    GameObject GameManager;
    GameObject BossSpawn;

	// Use this for initialization
	void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        BossSpawn = Instantiate(GameManager.GetComponent<GameManager>().GenerateABoss(), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1f), gameObject.transform.rotation).gameObject;
        BossSpawn.transform.parent = gameObject.transform;
        BossSpawn.GetComponent<BaseBoss>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
