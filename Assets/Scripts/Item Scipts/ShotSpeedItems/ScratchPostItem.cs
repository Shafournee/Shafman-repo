using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchPostItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletSpeed = .1f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Scratching Post";
        PickupText = "Shot Speed up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
