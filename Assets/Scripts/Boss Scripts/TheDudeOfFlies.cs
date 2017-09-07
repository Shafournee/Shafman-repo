using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDudeOfFlies : BaseBoss {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        BossMaxHealth = 10f;
        BossCurrentHealth = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }
}
