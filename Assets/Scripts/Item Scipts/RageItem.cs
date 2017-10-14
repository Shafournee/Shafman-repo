using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageItem : BaseItem {

    float DamageTakenOff = 0f;

	// Use this for initialization
	void Start () {
        base.Start();
        BulletDamage = 4f;
        FloorChangeHandler = ResetPlayerDamage;
        OnLoseHealth = LowerPlayerDamage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LowerPlayerDamage()
    {
        Player PScript = Player.GetComponent<Player>();
        if (PScript.BulletDamage <= 1f)
        {
            float DamageChange = PScript.BulletDamage - .5f;
            DamageTakenOff += DamageChange;
            PScript.BulletDamage = .5f;
        }
        else
        {
            Player.GetComponent<Player>().BulletDamage -= .5f;
            DamageTakenOff += .5f;
        }
    }

    void ResetPlayerDamage()
    {
        Player.GetComponent<Player>().BulletDamage += DamageTakenOff;
    }
}
