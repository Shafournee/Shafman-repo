using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFlyEnemy : BaseEnemy {

    private Transform PlayerPosition;
    private const float speed = .5f;
    public Transform EnemyBullet;
    bool IsShootingCoroutineRunning = false;
    //Find the unit vector on which the fly should shoot
    Vector2 VectorToPlayer;
    float MagnitudeOfVectorToPlayer;
    Vector2 UnitVectorToPlayer;
    //Define the bullet velocity
    float BulletVelocity = 2f;
    private Animator Animator;

    // Use this for initialization
    void Start()
    {
        Animator = GetComponent<Animator>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyHealth = 4;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the actual value for the unit vector
        VectorToPlayer = new Vector2(PlayerPosition.transform.position.x - gameObject.transform.position.x, PlayerPosition.transform.position.y - gameObject.transform.position.y).normalized;

        //Move towards the player if too far away, if close enough shoot towards the player
        float dist = Vector3.Distance(PlayerPosition.position, transform.position);
        Vector3 NormalVector = new Vector3(PlayerPosition.position.x - gameObject.transform.position.x, PlayerPosition.position.y - gameObject.transform.position.y, 1f).normalized;
        if (dist > 5)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = NormalVector * 2f;
        }
        else
        {
            if(IsShootingCoroutineRunning == false)
            {
                StartCoroutine(BigFlyShootingScript());
            }
        }
    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
        StartCoroutine(Knockback(DamageSourcePosition));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
    }

    

    IEnumerator BigFlyShootingScript()
    {
        IsShootingCoroutineRunning = true;
        //Set the attack animation to true
        
        const float TimeBetweenFiring = 1f;
        while (Vector3.Distance(PlayerPosition.position, transform.position) <= 5)
        {
            Animator.Play("BigFlyAttack");
            GameObject temporaryprojectile = Instantiate(EnemyBullet, new Vector2(transform.position.x, transform.position.y), transform.rotation).gameObject;
            Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
            temporaryprojectilebody.velocity = VectorToPlayer * BulletVelocity;
            Destroy(temporaryprojectile, 5f);
            yield return new WaitForSeconds(TimeBetweenFiring);
        }
        //Set the attact animation to false
        IsShootingCoroutineRunning = false;
    }
}
