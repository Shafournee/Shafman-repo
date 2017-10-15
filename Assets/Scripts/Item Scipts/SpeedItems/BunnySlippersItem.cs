using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnySlippersItem : BaseItem {

    

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Speed = 1f;
        BulletSpeed = .1f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Bunny Slippers";
        PickupText = "Speed up, bullet speed slightly up";
    }

    // Update is called once per frame
    void Update () {
		
	}

}
