using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        Speed = 1f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Whip";
        PickupText = "Speed up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
