using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilEyeItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = .5f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Cats Eye";
        PickupText = "Damage up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
