using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public string Side;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OnDoorEnter(Side);
        }
    }
}
