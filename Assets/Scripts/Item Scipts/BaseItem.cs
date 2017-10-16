using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour {

    protected GameObject Player;
    protected GameObject Canvas;

    protected int MaxHealth;
    protected int CurrentHealth;
    protected float BulletDamage;
    protected float Speed;
    protected float BulletSpeed;
    protected float BulletSize;
    protected string PickupText;
    protected string NameText;
    protected Sprite ItemSprite;

    //Event handlers
    protected ItemEventHandler OnHit;
    protected ItemEventHandler OnLoseHealth;
    protected ItemEventHandler FloorChangeHandler;
    protected ItemEventHandler OnShoot;

    // Use this for initialization
    public virtual void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        Canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player.GetComponent<Player>().MaxHealth += MaxHealth;
            Player.GetComponent<Player>().CurrentHealth += CurrentHealth;
            Player.GetComponent<Player>().BulletDamage += BulletDamage;
            Player.GetComponent<Player>().Speed += Speed;
            Player.GetComponent<Player>().MinimumTimeBetweenFiring -= BulletSpeed;
            Player.GetComponent<Player>().BulletSize += BulletSize;
            Player.GetComponent<Player>().ItemPickupAnimation(ItemSprite);
            Canvas.GetComponent<PlayerUI>().CallItemTextCoroutine(NameText, PickupText);
            Destroy(gameObject);

            Player.GetComponent<PlayerItemPickupEffects>().OnFloorChange += FloorChangeHandler;
            Player.GetComponent<PlayerItemPickupEffects>().OnHit += OnHit;
            Player.GetComponent<PlayerItemPickupEffects>().OnLoseHeath += OnLoseHealth;
            Player.GetComponent<PlayerItemPickupEffects>().OnShoot += OnShoot;
        }
    }
}
