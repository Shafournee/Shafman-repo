using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineItem : BaseItem {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = .1f;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Medicine";
        PickupText = "You feel sick";
        OnShoot = ChangeBulletSize;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void ChangeBulletSize()
    {
        Player.GetComponent<Player>().BulletSize = Random.Range(.5f, 3f);
    }
}
