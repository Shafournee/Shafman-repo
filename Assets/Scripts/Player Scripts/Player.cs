﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour {

    //Sprite renderer of the player
    SpriteRenderer SpriteHead;
    SpriteRenderer SpriteBody;

    //Defining the movement keys for the player
    KeyCode MoveUp = KeyCode.W;
    KeyCode MoveDown = KeyCode.S;
    KeyCode MoveLeft = KeyCode.A;
    KeyCode MoveRight = KeyCode.D;

    //Defining the shooting keys for the player
    KeyCode ShootUp = KeyCode.UpArrow;
    KeyCode ShootDown = KeyCode.DownArrow;
    KeyCode ShootLeft = KeyCode.LeftArrow;
    KeyCode ShootRight = KeyCode.RightArrow;

    KeyCode Exit = KeyCode.Escape;

    KeyCode P = KeyCode.P;

    //Make us able to call the Ball Prefab
    public Transform BallPrefab;
    //Provides a temporary game object for the pickup animation
    GameObject TemporaryItemPickup;

    //Defining the speed of the player
    [NonSerialized] public float Speed;
    //Defining the speed of the player's bullets
    float BulletSpeed;
    //Defining the current health of the player
    [NonSerialized] public int CurrentHealth;
    //Defines the maximum health of the player (heart containers)
    [NonSerialized] public int MaxHealth;
    //Set the damage of the bullet
    [NonSerialized] public float BulletDamage;
    //Used to set the firespeed of the player
    [NonSerialized] public float MinimumTimeBetweenFiring;

    [NonSerialized] public float BulletSize;

    //Sets whether the player is invincible after recently taking damage
    bool IsInvincible;
    //Make the player unable to move
    public bool PlayerCanMove;
    //Make the player unable to fire
    public bool PlayerCanShoot;
    //Checks if the player is colliding with any objects
    bool IsColliding;
    //Tells the game whether or not the character is shielded
    [NonSerialized] public bool IsShielded;

    //Provide a modifier seperate to the player's speed to make the projectiles able to be shot diagonally, like in isaac
    float BulletVelocityModifierx;
    float BulletVelocityModifiery;

    //The base color of the sprite
    public Color BaseColor;

    //Getting the rigidbody for the player object so that we can change velocity etc
    Rigidbody2D PlayerBody;

    //Get the animators
    Animator HeadAnimator;

    GameObject Shield;
    GameObject GameManager;


    // Use this for initialization
    void Start()
    {
        Speed = 5f;
        BulletSpeed = 8f;
        CurrentHealth = 6;
        MaxHealth = 6;
        BulletDamage = 1f;
        MinimumTimeBetweenFiring = .3f;
        IsInvincible = false;
        PlayerCanMove = true;
        PlayerCanShoot = true;
        BulletVelocityModifierx = 0f;
        BulletVelocityModifiery = 0f;
        BulletSize = 1f;

        SpriteHead = transform.Find("Head").GetComponent<SpriteRenderer>();
        SpriteBody = transform.Find("Body").GetComponent<SpriteRenderer>();

        //Getting the rigidbody for the player object so that we can change velocity etc
        PlayerBody = GetComponent<Rigidbody2D>();

        BaseColor = new Color(1f, 1f, 1f, 1f);

        //Run the playershooting function
        StartCoroutine(ShootingCoroutine());

        DontDestroyOnLoad(gameObject);

        GameManager = GameObject.FindGameObjectWithTag("GameManager");

        IsShielded = false;
        Shield = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsInvincible)
        {
            SpriteHead.color = BaseColor;
            SpriteBody.color = BaseColor;
        }

        //Run the playermovement function
        if (PlayerCanMove)
        {
            PlayerMovement();
        }

        //Quits the game
        if (Input.GetKeyDown(Exit))
        {
            GameManager.GetComponent<GameManager>().PauseGame();
        }

        //Ensures that modifiable player values don't go too high or low
        RestrictPlayerStats();

        if (Input.GetKeyDown(P))
        {
            Debug.Log(BulletDamage);
        }

        ActivateTheShieldSprite();

    }

    void PlayerMovement()
    {
        //Make the player move up
        if (Input.GetKey(MoveUp))
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, Speed);
            BulletVelocityModifiery = 20f;
            gameObject.GetComponent<PlayerAnimationManager>().BodyAnimationManager(BodyDirection.Up);
        }

        //Make the player move down
        else if (Input.GetKey(MoveDown))
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -Speed);
            BulletVelocityModifiery = -20f;
            gameObject.GetComponent<PlayerAnimationManager>().BodyAnimationManager(BodyDirection.Down);
        }

        //Make the player's vertical velocity go to zero if pressing no vertical movement keys
        else
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, 0f);
            BulletVelocityModifiery = 0f;
        }

        //Make the player move left
        if (Input.GetKey(MoveLeft))
        {
            PlayerBody.velocity = new Vector2(-Speed, PlayerBody.velocity.y);
            BulletVelocityModifierx = -20f;
            if (Input.GetKey(MoveUp) == false && Input.GetKey(MoveDown) == false)
            {
                gameObject.GetComponent<PlayerAnimationManager>().BodyAnimationManager(BodyDirection.Left);
            }
        }

        //Make the player move right
        else if (Input.GetKey(MoveRight))
        {
            PlayerBody.velocity = new Vector2(Speed, PlayerBody.velocity.y);
            BulletVelocityModifierx = 20f;
            if (Input.GetKey(MoveUp) == false && Input.GetKey(MoveDown) == false)
            {
                gameObject.GetComponent<PlayerAnimationManager>().BodyAnimationManager(BodyDirection.Right);
            }
        }

        //Make the player's horizontal velocity go to zero if pressing no horizontal movement keys
        else
        {
            PlayerBody.velocity = new Vector2(0f, PlayerBody.velocity.y);
            BulletVelocityModifierx = 0f;
        }
        //Reset the body to standing if movement keys are released
        if (Input.GetKeyUp(MoveUp) || Input.GetKeyUp(MoveDown) || Input.GetKeyUp(MoveLeft) || Input.GetKeyUp(MoveRight))
        {
            gameObject.GetComponent<PlayerAnimationManager>().BodyAnimationManager(BodyDirection.Static);
        }


    }

    //Check if the player is colliding with anything, and if so return true or false
    private void OnCollisionStay2D(Collision2D collision)
    {
        IsColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsColliding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    //Damages the player if walking on spikes
    private void OnTriggerStay2D(Collider2D collider)
    {
        //if colliding with a spike and am not invincible take damage and set invincibility
        if (collider.tag == "Spike")
        {
            OnHit(collider.transform.position);
        }
    }

    //Creating a single Function that runs all the Coroutines
    public void OnHit(Vector3 DamageSourcePosition)
    {
        if (IsShielded)
        {
            IsShielded = false;
            StartCoroutine(InvincibilityCoroutine());
        }
        else if (!IsInvincible)
        {
            LoseHealth();
            //StartCoroutine(Knockback(DamageSourcePosition));
            StartCoroutine(InvincibilityCoroutine());
        }

    }

    //Function that handles the player losing health as well as ending the game
    private void LoseHealth()
    {
        CurrentHealth--;
        GetComponent<PlayerItemPickupEffects>().CallOnLoseHealth();
        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }



    //Function that handles the player gaining health
    public void GainHealth(int HeartValue)
    {
        if (CanGainHealth() == true)
        {
            CurrentHealth += HeartValue;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

        }
    }

    private void RestrictPlayerStats()
    {
        //Ensure the player can't go higher than 12 hearts
        if (MaxHealth > 24)
        {
            MaxHealth = 24;
        }

        if (BulletDamage > 7f)
        {
            BulletDamage = 4f;
        }
        if (BulletDamage < .3f)
        {
            BulletDamage = .3f;
        }

        if (MinimumTimeBetweenFiring < .1f)
        {
            MinimumTimeBetweenFiring = .1f;
        }
        if (MinimumTimeBetweenFiring > 1.2f)
        {
            MinimumTimeBetweenFiring = 1.2f;
        }

        if (Speed > 10f)
        {
            Speed = 10f;
        }
        if (Speed < 3f)
        {
            Speed = 3f;
        }

        if (BulletSpeed > 10f)
        {
            BulletSpeed = 10f;
        }
        if (BulletSpeed < 7f)
        {
            BulletSpeed = 7f;
        }

        if (BulletSize > 2f)
        {
            BulletSize = 2f;
        }
        if (BulletSize < .5f)
        {
            BulletSize = .5f;
        }
    }

    //Checks if the player is able to gain health
    public bool CanGainHealth()
    {
        return CurrentHealth < MaxHealth;
    }

    //The function for shooting
    IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            //make sure the player can shoot, and if they can't just return nothing
            if (PlayerCanShoot)
            {
                gameObject.GetComponent<PlayerItemPickupEffects>().CallOnShoot();
                //Provides a divisor for the bullet velocity modifier by the player so I don't have to change each individual one lol
                const int PlayerVelocityBulletModifier = 20;
                //Fires bullets upwards
                if (Input.GetKey(ShootUp))
                {
                    gameObject.GetComponent<PlayerAnimationManager>().HeadAnimationManager(HeadDirection.Up);
                    GameObject temporaryprojectile = Instantiate(BallPrefab, new Vector2(transform.position.x, transform.position.y + .5f), transform.rotation).gameObject;
                    Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
                    temporaryprojectile.transform.localScale = new Vector3(1f * BulletSize, 1f * BulletSize, 1f * BulletSize);
                    if (Input.GetKey(MoveDown))
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletVelocityModifierx / PlayerVelocityBulletModifier, BulletSpeed);
                    }
                    else if (IsColliding == false)
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletVelocityModifierx / PlayerVelocityBulletModifier, PlayerBody.velocity.y / 4 + BulletSpeed);
                    }
                    else
                    {
                        temporaryprojectilebody.velocity = new Vector2(0, BulletSpeed);
                    }

                    Destroy(temporaryprojectile, 5f);
                    yield return new WaitForSeconds(MinimumTimeBetweenFiring);
                }

                //Fires bullets downwards
                else if (Input.GetKey(ShootDown))
                {
                    gameObject.GetComponent<PlayerAnimationManager>().HeadAnimationManager(HeadDirection.Down);
                    GameObject temporaryprojectile = Instantiate(BallPrefab, new Vector2(transform.position.x, transform.position.y - .8f), transform.rotation).gameObject;
                    Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
                    temporaryprojectile.transform.localScale = new Vector3(1f * BulletSize, 1f * BulletSize, 1f * BulletSize);
                    if (Input.GetKey(MoveUp))
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletVelocityModifierx / PlayerVelocityBulletModifier, -BulletSpeed);
                    }
                    else if (IsColliding == false)
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletVelocityModifierx / PlayerVelocityBulletModifier, PlayerBody.velocity.y / 4 - BulletSpeed);
                    }
                    else
                    {
                        temporaryprojectilebody.velocity = new Vector2(0, -BulletSpeed);
                    }
                    Destroy(temporaryprojectile, 5f);
                    yield return new WaitForSeconds(MinimumTimeBetweenFiring);
                }

                //Fires bullets leftwards
                else if (Input.GetKey(ShootLeft))
                {
                    gameObject.GetComponent<PlayerAnimationManager>().HeadAnimationManager(HeadDirection.Left);
                    GameObject temporaryprojectile = Instantiate(BallPrefab, new Vector2(transform.position.x - .5f, transform.position.y), transform.rotation).gameObject;
                    Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
                    temporaryprojectile.transform.localScale = new Vector3(1f * BulletSize, 1f * BulletSize, 1f * BulletSize);
                    if (Input.GetKey(MoveRight))
                    {
                        temporaryprojectilebody.velocity = new Vector2(-BulletSpeed, BulletVelocityModifiery / PlayerVelocityBulletModifier);
                    }
                    else if (IsColliding == false)
                    {
                        temporaryprojectilebody.velocity = new Vector2(PlayerBody.velocity.x / 4 - BulletSpeed, BulletVelocityModifiery / PlayerVelocityBulletModifier);
                    }
                    else
                    {
                        temporaryprojectilebody.velocity = new Vector2(-BulletSpeed, 0);
                    }

                    Destroy(temporaryprojectile, 5f);
                    yield return new WaitForSeconds(MinimumTimeBetweenFiring);
                }

                //Fires bullets rightwards
                else if (Input.GetKey(ShootRight))
                {
                    gameObject.GetComponent<PlayerAnimationManager>().HeadAnimationManager(HeadDirection.Right);
                    GameObject temporaryprojectile = Instantiate(BallPrefab, new Vector2(transform.position.x + .5f, transform.position.y), transform.rotation).gameObject;
                    Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
                    temporaryprojectile.transform.localScale = new Vector3(1f * BulletSize, 1f * BulletSize, 1f * BulletSize);
                    if (Input.GetKey(MoveLeft))
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletSpeed, BulletVelocityModifiery / PlayerVelocityBulletModifier);
                    }
                    else if (IsColliding == false)
                    {
                        temporaryprojectilebody.velocity = new Vector2(PlayerBody.velocity.x / 4 + BulletSpeed, BulletVelocityModifiery / PlayerVelocityBulletModifier);
                    }
                    else
                    {
                        temporaryprojectilebody.velocity = new Vector2(BulletSpeed, 0);
                    }
                    Destroy(temporaryprojectile, 5f);
                    yield return new WaitForSeconds(MinimumTimeBetweenFiring);
                }
                else
                {
                    gameObject.GetComponent<PlayerAnimationManager>().HeadAnimationManager(HeadDirection.Static);
                    yield return null;
                }

            }
            yield return null;
        }
    }

    //Coroutine to make the player get pushed back from spikes
    /*
    IEnumerator Knockback(Vector3 ColliderPosition)
    {
        PlayerCanMove = false;
        const float CoroutineLoopTimeIncrementer = .2f;
        const float CoroutineTotalLoopTime = 1f;
        float CoroutineIncrementedLoopTime = 0f;
        const float KnockbackDistance = -.2f;
        while (CoroutineIncrementedLoopTime <= CoroutineTotalLoopTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, ColliderPosition, KnockbackDistance);
            CoroutineIncrementedLoopTime += CoroutineLoopTimeIncrementer;
            yield return null;
        }
        PlayerCanMove = true;
    }
    */

    //Coroutine to make the player blink and keep invincibility
    IEnumerator InvincibilityCoroutine()
    {
        const float FlashRate = 0.15f;
        const float InvincibilityTime = 1.00f;
        IsInvincible = true;
        Color Transparent = new Color(BaseColor.r, BaseColor.g, BaseColor.b, BaseColor.a - 0.45f);
        float TimePassedSoFar = 0f;
        while (TimePassedSoFar < InvincibilityTime)
        {
            if (SpriteHead.color == BaseColor)
            {
                SpriteHead.color = Transparent;
                SpriteBody.color = Transparent;
            }
            else
            {
                SpriteHead.color = BaseColor;
                SpriteBody.color = BaseColor;
            }
            TimePassedSoFar += FlashRate;
            yield return new WaitForSeconds(FlashRate);
        }
        SpriteHead.color = BaseColor;
        SpriteBody.color = BaseColor;
        IsInvincible = false;
    }



    //Make the player play the zelda animation
    public void ItemPickupAnimation(Sprite ItemSprite)
    {
        StartCoroutine(ItemPickupCoroutine(ItemSprite));
    }

    IEnumerator ItemPickupCoroutine(Sprite ItemSprite)
    {
        PlayerCanMove = false;
        PlayerCanShoot = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        TemporaryItemPickup = new GameObject("ItemPickupSprite");
        TemporaryItemPickup.AddComponent<SpriteRenderer>();
        TemporaryItemPickup.GetComponent<SpriteRenderer>().sprite = ItemSprite;
        TemporaryItemPickup.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .5f, 1f);
        TemporaryItemPickup.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 2;
        //Play player pickup animation too
        yield return new WaitForSeconds(2f);
        PlayerCanMove = true;
        PlayerCanShoot = true;
        Destroy(TemporaryItemPickup);
    }

    private void ActivateTheShieldSprite()
    {
        if (IsShielded)
        {
            Shield.SetActive(true);
        }
        else
        {
            Shield.SetActive(false);
        }
    }

}
