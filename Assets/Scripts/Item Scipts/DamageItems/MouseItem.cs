using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = .7f;
        Speed = -.2f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Mouse";
        PickupText = "Damage up, Speed down";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
