using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFlyEnemy : BaseEnemy {

    private Transform PlayerPosition;
    private const float speed = 2f;

	// Use this for initialization
	void Start () {

        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyHealth = 3;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 NormalVector = new Vector3(PlayerPosition.position.x - gameObject.transform.position.x, PlayerPosition.position.y - gameObject.transform.position.y, 1f).normalized;
        gameObject.GetComponent<Rigidbody2D>().velocity = NormalVector * 3f;
	}

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
        StartCoroutine(Knockback(DamageSourcePosition));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
    }


}
