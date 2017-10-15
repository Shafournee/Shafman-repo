using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFoodItem : BaseItem {


    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = .5f;
        MaxHealth = 2;
        CurrentHealth = 2;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Cat food";
        PickupText = "Health up, Damage up";
    }

    // Update is called once per frame
    void Update () {
		
	}

}
