using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannedFoodItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxHealth = 4;
        CurrentHealth = 1;
        Speed = -1f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Canned Food";
        PickupText = "Lots of health, but you feel fat";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
