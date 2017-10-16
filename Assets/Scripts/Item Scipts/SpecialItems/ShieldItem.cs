using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Shield";
        PickupText = "You feel protected";
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        Player.GetComponent<PlayerItemPickupEffects>().IsShielded = true;
        Player.GetComponent<PlayerItemPickupEffects>().ItemPickupEffects();
    }

}
