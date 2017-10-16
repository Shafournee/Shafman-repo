using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrapItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = .4f;
        Speed = .2f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Mouse Trap";
        PickupText = "Damage up, Speed up";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
