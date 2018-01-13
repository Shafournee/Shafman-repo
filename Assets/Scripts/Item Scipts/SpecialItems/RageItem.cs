using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageItem : BaseItem {

    float DamageTakenOff = 0f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        BulletDamage = 2f;
        FloorChangeHandler = ResetPlayerDamage;
        OnLoseHealth = LowerPlayerDamage;
        ItemSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        NameText = "Roid Rage";
        PickupText = "Damage way up, but don't get hit";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LowerPlayerDamage()
    {
        Player PScript = Player.GetComponent<Player>();
        if (PScript.BulletDamage <= 2f)
        {
            float DamageChange = PScript.BulletDamage - 1f;
            DamageTakenOff += DamageChange;
            PScript.BulletDamage = 1f;
        }
        else
        {
            Player.GetComponent<Player>().BulletDamage -= 1f;
            DamageTakenOff += 1f;
        }
    }

    void ResetPlayerDamage()
    {
        Player.GetComponent<Player>().BulletDamage += DamageTakenOff;
    }
}
