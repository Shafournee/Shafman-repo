using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldHeartItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxHealth = 4;
        CurrentHealth = 4;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Gold Heart";
        PickupText = "You feel like a better person";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
