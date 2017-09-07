using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    Transform BulletPosition;
    //Get the player to get the bullet damage
    float PlayerBulletDamage;

	// Use this for initialization
	void Start () {
        BulletPosition = gameObject.transform;
        PlayerBulletDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().BulletDamage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Ensure the bullets aren't destroyed on objects you don't want them destroyed on
        if (collider.tag != "PlayerBullet" && collider.tag != "EnemyBullet")
        {
            if (gameObject.tag == "PlayerBullet" && collider.tag == "Player")
            {
                //Ignore the player bullet hitting the player
            }
            else if (gameObject.tag == "EnemyBullet" && collider.tag == "Enemy")
            {
                //Ignore the enemy bullets hitting the enemies
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
        
        //Make the Player Bullets damage the enemy
        if (gameObject.tag == "PlayerBullet" && collider.tag == "Enemy")
        {
            collider.GetComponent<BaseEnemy>().OnHit(transform.position, PlayerBulletDamage);
            
        }
        //Make the Player Bullets damage bosses
        if(gameObject.tag == "PlayerBullet" && collider.tag == "Boss")
        {
            collider.GetComponent<BaseBoss>().OnHit(transform.position, PlayerBulletDamage);
        }
        //Make the Enemy Bullets damage the player
        if (gameObject.tag == "EnemyBullet" && collider.tag == "Player")
        {
            collider.GetComponent<Player>().OnHit(gameObject.transform.position);
        }
    }

}
