using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : BaseEnemy {

    //Get the transform of the enemy bullet
    public Transform EnemyBullet;
    SpriteRenderer Sprite;
    float EnemyHealth = 3;
    //Determines the position of the where the bullet comes out if facing left or right
    int BulletPosition = -1;
    //Determines the velocity of the bullet if facing left or right
    float ProjectileVelocity = -2f;

    // Use this for initialization
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        DirectionTurretIsFacing();
        
        StartCoroutine(StationaryTurretEnemyShootingScript());
    }

    // Update is called once per frame
    void Update()
    {




    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        EnemyHealth = EnemyHealth - PlayerBulletDamage;
        if (EnemyHealth <= 0)
        {
            //Destroy the enemy and run the script to see if all the enemies in a room are dead
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenDoorsOnEnemyDeaths();
        }
    }


    IEnumerator StationaryTurretEnemyShootingScript()
    {
        const float TimeBetweenFiring = 1f;
        while (true)
        {
            GameObject temporaryprojectile = Instantiate(EnemyBullet, new Vector2(transform.position.x + BulletPosition, transform.position.y), transform.rotation).gameObject;
            Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
            temporaryprojectilebody.velocity = new Vector2(ProjectileVelocity, 0f);
            Destroy(temporaryprojectile, 5f);
            yield return new WaitForSeconds(TimeBetweenFiring);
        }


    }

    //Determine the direction the enemy should face, and where the bullets shoot from
    public int DirectionTurretIsFacing()
    {
        //Finding the center of the current room
        GameObject CurrentRoom = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().CurrentRoomFunction();
        float CenterOfRoomxValue = CurrentRoom.transform.position.x;
        //Find if the distance to the center is negative or positive
        float DistanceToCenter = gameObject.transform.position.x - CenterOfRoomxValue;
        if(DistanceToCenter < 0)
        {
            Sprite.flipX = true;
            BulletPosition = 1;
            ProjectileVelocity = 2f;
        }
        else
        {
            BulletPosition = -1;
            ProjectileVelocity = -2f;
        }
        return BulletPosition;
    }
}
