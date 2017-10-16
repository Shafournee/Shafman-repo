using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Speed = 1f;
        MaxHealth = 2;
        CurrentHealth = 2;
        BulletSpeed = .1f;
        BulletDamage = .4f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Magic Mushroom";
        PickupText = "All stats up!";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
