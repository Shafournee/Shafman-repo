using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : MonoBehaviour {

    protected float BossMaxHealth;
    protected float BossCurrentHealth;
    protected bool DoesDamageOnHit;
    public string BossName;
    public Sprite BossSprite;
    GameObject Canvas;
    

	// Use this for initialization
	protected virtual void Start () {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        BossCurrentHealth = BossCurrentHealth - PlayerBulletDamage;
        Canvas.GetComponent<PlayerUI>().BossHealth(BossMaxHealth);
        if (BossCurrentHealth <= 0)
        {
            //Destroy the enemy and run the script to see if all the enemies in a room are dead
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenTheExitOnBossDeath();
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenDoorsOnEnemyDeaths();
            //Hide the boss healthbar on death
            Canvas.GetComponent<PlayerUI>().HideTheBossHealthBar();
        }
    }

    //Damages the player on contact if the boss is meant to by changing a bool in the child boss script

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && DoesDamageOnHit == true)
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
    }

}
