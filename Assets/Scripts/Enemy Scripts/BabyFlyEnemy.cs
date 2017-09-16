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
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, PlayerPosition.position, step);
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
