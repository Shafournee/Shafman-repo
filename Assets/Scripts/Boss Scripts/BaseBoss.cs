using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoss : MonoBehaviour {

    protected float BossMaxHealth;
    protected float BossCurrentHealth;
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
        }
    }
}
