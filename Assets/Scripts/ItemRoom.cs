using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoom : MonoBehaviour {

    
    
    GameObject GameManager;

	// Use this for initialization
	void Start () {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        //If there are no items left in the list 
        GameObject ItemInRoom = Instantiate(GameManager.GetComponent<GameManager>().GenerateAGameObject(), new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1f), gameObject.transform.rotation).gameObject;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
