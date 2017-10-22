using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Minimap";
        PickupText = "Shows you the way";
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        Player.GetComponent<PlayerItemPickupEffects>().MinimapEffect();
    }
}