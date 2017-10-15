using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletSpeed = .1f;
        Speed = -.2f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Bird";
        PickupText = "Shot Speed up, Speed down";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
