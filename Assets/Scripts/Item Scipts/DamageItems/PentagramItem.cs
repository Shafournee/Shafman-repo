using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = 3f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Pentagram";
        PickupText = "Damage way up, but you're cursed";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
