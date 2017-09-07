using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfHeartPickup : HeartPickup {

	// Use this for initialization
	new void Start () {
        base.Start();
        HealthValue = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
