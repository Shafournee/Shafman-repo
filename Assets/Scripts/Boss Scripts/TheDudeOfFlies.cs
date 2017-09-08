using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDudeOfFlies : BaseBoss {

    Rigidbody2D Rigidbody;
    public GameObject SmallFly;
    public GameObject BigFly;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        BossMaxHealth = 10f;
        BossCurrentHealth = 10f;
        DoesDamageOnHit = true;
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        ChangeStartDirection();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeStartDirection()
    {
        float StartDirectionx = Mathf.Sign(Random.Range(-1, 1));
        float StartDirectiony = Mathf.Sign(Random.Range(-1, 1));
        Vector2 UnitVectorDirection = new Vector2(.45f * StartDirectionx, .45f * StartDirectiony).normalized;
        Rigidbody.velocity = UnitVectorDirection * 3f;
    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }


}
