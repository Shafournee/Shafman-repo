using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyDirectionDetector : MonoBehaviour {

    public bool IsTriggered;

	// Use this for initialization
	void Start () {
        IsTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag != "Player")
        {
            IsTriggered = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsTriggered = false;
    }
}
