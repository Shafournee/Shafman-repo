using System.Collections;
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

    KeyCode TellBool = KeyCode.B;

    enum BodyDirection { Up, Down, Left, Right, Static }

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
    private float NextFire;

    //Turns the player see through and makes them walk through rocks and over spikes
    [NonSerialized] public bool IsGhost;
    //Sets whether the player is invincible after recently taking damage
    bool IsInvincible;
    //Make the player unable to move
    bool PlayerCanMove;
    //Make the player unable to fire
    public bool PlayerCanShoot;
    //Checks if the player is colliding with any objects
    bool IsColliding;

    //Provide a modifier seperate to the player's speed to make the projectiles able to be shot diagonally, like in isaac
    float BulletVelocityModifierx;
    float BulletVelocityModifiery;

    //The base color of the sprite
    public Color BaseColor;

    //Getting the rigidbody for the player object so that we can change velocity etc
    Rigidbody2D PlayerBody;

    //Get the animators
    Animator HeadAnimator;
    Animator BodyAnimator;

    GameObject GameManager;


    // Use this for initialization
    void Start()
    {
        Speed = 5f;
        BulletSpeed = 8f;
        CurrentHealth = 6;
        MaxHealth = 6;
        BulletDamage = 1f;
        MinimumTimeBetweenFiring = .5f;
        NextFire = 0.0f;
        IsGhost = false;
        IsInvincible = false;
        PlayerCanMove = true;
        PlayerCanShoot = true;
        BulletVelocityModifierx = 0f;
        BulletVelocityModifiery = 0f;
        BulletSize = 1f;

        HeadAnimator = GetComponentsInChildren<Animator>()[0];
        BodyAnimator = transform.Find("Body").GetComponent<Animator>();

        SpriteHead = transform.Find("Head").GetComponent<SpriteRenderer>();
        SpriteBody = transform.Find("Body").GetComponent<SpriteRenderer>();

        //Getting the rigidbody for the player object so that we can change velocity etc
        PlayerBody = GetComponent<Rigidbody2D>();

        BaseColor = new Color(1f, 1f, 1f, 1f);

        //Run the playershooting function
        StartCoroutine(ShootingCoroutine());

        DontDestroyOnLoad(gameObject);

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
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
        //Ensure the player can't go higher than 12 hearts
        if (MaxHealth > 24)
        {
            MaxHealth = 24;
        }
    }

    void PlayerMovement()
    {
        //Make the player move up
        if (Input.GetKey(MoveUp))
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, Speed);
            BulletVelocityModifiery = 20f;
            BodyAnimationManager(BodyDirection.Up);
        }

        //Make the player move down
        else if (Input.GetKey(MoveDown))
        {
            PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -Speed);
            BulletVelocityModifiery = -20f;
            BodyAnimationManager(BodyDirection.Down);
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
            BodyAnimationManager(BodyDirection.Left);
        }

        //Make the player move right
        else if (Input.GetKey(MoveRight))
        {
            PlayerBody.velocity = new Vector2(Speed, PlayerBody.velocity.y);
            BulletVelocityModifierx = 20f;
            BodyAnimationManager(BodyDirection.Right);
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
            BodyAnimationManager(BodyDirection.Static);
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
        if (!IsInvincible)
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

    //Checks if the player is able to gain health
    public bool CanGainHealth()
    {
        return CurrentHealth < MaxHealth;
    }

    //The function for shooting
    IEnumerator ShootingCoroutine()
    {
        while(true)
        {
            //make sure the player can shoot, and if they can't just return nothing
            if(PlayerCanShoot)
            {
                //Provides a divisor for the bullet velocity modifier by the player so I don't have to change each individual one lol
                const int PlayerVelocityBulletModifier = 20;
                //Fires bullets upwards
                if (Input.GetKey(ShootUp))
                {
                    HeadAnimationManager("Up");
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
                    HeadAnimationManager("Down");
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
                    HeadAnimationManager("Left");
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
                    HeadAnimationManager("Right");
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
                    yield return null;
                    HeadAnimationManager("Static");
                }
                
            }
            yield return null;
        }
    }

    //Coroutine to make the player get pushed back from spikes
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

    public void ItemPickupEffects()
    {
        if (IsGhost)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerFlying");
            BaseColor = new Color(BaseColor.r, BaseColor.g, BaseColor.b, 0.6f);
        }
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

    //Handles the direction and animations of the head
    private void HeadAnimationManager(string HeadDirection)
    {
        if(HeadDirection == "Down")
        {
            HeadAnimator.Play("ShootDown");
        }
        else if (HeadDirection == "Right")
        {
            HeadAnimator.Play("ShootRight");
        }
        else if (HeadDirection == "Left")
        {
            HeadAnimator.Play("ShootLeft");
        }
        else if (HeadDirection == "Up")
        {
            HeadAnimator.Play("ShootUp");
        }
        else if (HeadDirection == "Static")
        {

        }
        else
        {
            HeadAnimator.Play("DefaultHead");
        }
        
    }

    //Handles the direction and animations of the body
    private void BodyAnimationManager(BodyDirection MoveDirection)
    {
        if(MoveDirection == BodyDirection.Down)
        {
            BodyAnimator.Play("WalkDown");
        }
        else if(MoveDirection == BodyDirection.Up)
        {

        }
        else if (MoveDirection == BodyDirection.Left)
        {

        }
        else if (MoveDirection == BodyDirection.Right)
        {

        }
        else if (MoveDirection == BodyDirection.Static)
        {
            BodyAnimator.Play("Standing");
        }
    }
}
