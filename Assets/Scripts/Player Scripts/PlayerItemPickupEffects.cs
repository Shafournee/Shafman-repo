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
    public event ItemEventHandler OnShoot;

    //Turns the player see through and makes them walk through rocks and over spikes
    //[NonSerialized] public bool IsGhost;
    [NonSerialized] public bool RevealedMinimap;
    //[NonSerialized] public bool IsShielded;
    GameObject Minimap;

    // Use this for initialization
    void Start () {
        //IsGhost = false;
        RevealedMinimap = false;
        FindMinimap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FindMinimap()
    {
        Minimap = GameObject.FindGameObjectWithTag("Minimap");
    }

    public void GhostEffect()
    {
        Color ColorBasis = gameObject.GetComponent<Player>().BaseColor;
        gameObject.layer = LayerMask.NameToLayer("PlayerFlying");
        gameObject.GetComponent<Player>().BaseColor = new Color(ColorBasis.r, ColorBasis.g, ColorBasis.b, 0.6f);
    }

    public void MinimapEffect()
    {
        Minimap.GetComponent<Minimap>().RevealTheMinimap = true;
        Minimap.GetComponent<Minimap>().RevealTheMinimapFunction();
        RevealedMinimap = true;
    }

    public void ShieldEffect()
    {
        GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().PlayerHasShield = true;
        gameObject.GetComponent<Player>().IsShielded = true;
    }

    /*
    public void ItemPickupEffects()
    {
        if (IsGhost)
        {
            Color ColorBasis = gameObject.GetComponent<Player>().BaseColor;
            gameObject.layer = LayerMask.NameToLayer("PlayerFlying");
            gameObject.GetComponent<Player>().BaseColor = new Color(ColorBasis.r, ColorBasis.g, ColorBasis.b, 0.6f);
        }

        if (RevealedMinimap)
        {
            Minimap.GetComponent<Minimap>().RevealTheMinimap = true;
            Minimap.GetComponent<Minimap>().RevealTheMinimapFunction();
        }

        if (IsShielded)
        {
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().PlayerHasShield = true;
            gameObject.GetComponent<Player>().IsShielded = true;
        }
    }
    */

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

    public void CallOnShoot()
    {
        if(OnShoot != null)
        {
            OnShoot.Invoke();
        }
    }
}
