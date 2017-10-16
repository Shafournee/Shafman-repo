using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatnipItem : BaseItem {

	// Use this for initialization
	public override void Start () {
        base.Start();
        BulletSize = 2f;
        BulletDamage = .5f;
        BulletSpeed = -.5f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Catnip";
        PickupText = "Damage way up, shot speed way down, big tears";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
