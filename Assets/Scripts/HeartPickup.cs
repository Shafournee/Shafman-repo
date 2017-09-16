using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour {

    protected int HealthValue;
    public bool IsColliding = true;
    SpriteRenderer HeartSprite;
    Color BecomeVisable = new Color(1f, 1f, 1f, 1f);

    // Use this for initialization
    protected void Start () {
        HeartSprite = GetComponent<SpriteRenderer>();
        HeartSprite.color = BecomeVisable;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collision.collider.GetComponent<Player>().CanGainHealth() == true)
        {
            collision.collider.GetComponent<Player>().GainHealth(HealthValue);
            Destroy(gameObject);
        }
        /*
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            IsColliding = true;
            HeartSprite.color = SpawnTransparent;
        }
        else if (collision.collider.tag != "Player")
        {
            IsColliding = false;
            HeartSprite.color = BecomeVisable;
        }
        */
    }
}
