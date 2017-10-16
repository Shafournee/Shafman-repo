using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastboiItem : BaseItem
{
    public override void Start()
    {
        base.Start();
        Speed = 1f;
        BulletSpeed = .2f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Fastboi";
        PickupText = "u feel rely fast";
    }
	
}