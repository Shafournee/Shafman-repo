using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerItemPickupEffects : MonoBehaviour {

    //Turns the player see through and makes them walk through rocks and over spikes
    [NonSerialized] public bool IsGhost;

    // Use this for initialization
    void Start () {
        IsGhost = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ItemPickupEffects()
    {
        if (IsGhost)
        {
            Color ColorBasis = gameObject.GetComponent<Player>().BaseColor;
            gameObject.layer = LayerMask.NameToLayer("PlayerFlying");
            gameObject.GetComponent<Player>().BaseColor = new Color(ColorBasis.r, ColorBasis.g, ColorBasis.b, 0.6f);
        }
    }
}
