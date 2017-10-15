using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBallItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletSpeed = .07f;
        Speed = .2f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Yarn";
        PickupText = "Shot Speed up, Speed up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
