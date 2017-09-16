using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    protected float EnemyHealth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        EnemyHealth = EnemyHealth - PlayerBulletDamage;
        if (EnemyHealth <= 0)
        {
            //Destroy the enemy and run the script to see if all the enemies in a room are dead
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenDoorsOnEnemyDeaths();
        }
    }

    protected virtual IEnumerator Knockback(Vector3 ColliderPosition)
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


