using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHeartPickup : HeartPickup {

	// Use this for initialization
	new void Start () {
        base.Start();
        HealthValue = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

}
