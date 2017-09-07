using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFlyEnemy : BaseEnemy {

    private Transform PlayerPosition;
    private const float speed = 2f;
    private float EnemyHealth = 3;

	// Use this for initialization
	void Start () {

        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, PlayerPosition.position, step);
	}

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        StartCoroutine(Knockback(DamageSourcePosition));
        EnemyHealth = EnemyHealth - PlayerBulletDamage;
        if (EnemyHealth <= 0)
        {
            
            //Destroy the enemy and run the script to see if all the enemies in a room are dead
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenDoorsOnEnemyDeaths();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
    }

    IEnumerator Knockback(Vector3 ColliderPosition)
    {
        const float CoroutineLoopTimeIncrementer = .2f;
        const float CoroutineTotalLoopTime = 1f;
        float CoroutineIncrementedLoopTime = 0f;
        const float KnockbackDistance = -.1f;
        while (CoroutineIncrementedLoopTime <= CoroutineTotalLoopTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, ColliderPosition, KnockbackDistance);
            CoroutineIncrementedLoopTime += CoroutineLoopTimeIncrementer;
            yield return null;
        }
    }

}
