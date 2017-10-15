using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoyMilkItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletSpeed = 1f;
        BulletDamage = -1.5f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Soy Milk";
        PickupText = "Shot Speed way up, Damage down";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
