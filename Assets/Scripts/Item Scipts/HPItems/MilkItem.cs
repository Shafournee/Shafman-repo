﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        MaxHealth = 2;
        CurrentHealth = 2;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Milk";
        PickupText = "Health up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
