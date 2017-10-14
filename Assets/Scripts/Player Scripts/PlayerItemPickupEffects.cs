using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public delegate void ItemEventHandler();

public class PlayerItemPickupEffects : MonoBehaviour {

    public event ItemEventHandler OnHit;
    public event ItemEventHandler OnFloorChange;
    public event ItemEventHandler OnLoseHeath;

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

    public void CallOnLoseHealth()
    {
        if(OnLoseHeath != null)
        {
            OnLoseHeath.Invoke();
        }        
    }

    public void CallOnFloorChange()
    {
        if(OnFloorChange != null)
        {
            OnFloorChange.Invoke();
        }   
    }
}
