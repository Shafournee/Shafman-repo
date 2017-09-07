using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalTurretEnemy : BaseEnemy {

    float EnemyHealth = 3;
    Transform PlayerPosition;
    bool IsShootingCoroutineRunning = false;
    public Transform EnemyBullet;
    //Find the unit vector on which the fly should shoot
    Vector2 VectorToPlayer;
    //Define the bullet velocity
    float BulletVelocity = 4f;
    Vector2 PlayerVelocityModifierToPredictPlayerPosition;

    // Use this for initialization
    void Start () {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {

        //Get the actual value for the unit vector
        VectorToPlayer = new Vector2(PlayerPosition.transform.position.x - gameObject.transform.position.x, PlayerPosition.transform.position.y - gameObject.transform.position.y);


        float dist = Vector3.Distance(PlayerPosition.position, transform.position);
        if (dist < 7)
        {
            if (IsShootingCoroutineRunning == false)
            {
                StartCoroutine(DirectionalTurretShootingScript());
            }
        }
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

    

    IEnumerator DirectionalTurretShootingScript()
    {
        IsShootingCoroutineRunning = true;
        const float TimeBetweenFiring = .5f;
        while (Vector3.Distance(PlayerPosition.position, transform.position) <= 7)
        {
            GameObject temporaryprojectile = Instantiate(EnemyBullet, new Vector2(transform.position.x, transform.position.y), transform.rotation).gameObject;
            Rigidbody2D temporaryprojectilebody = temporaryprojectile.GetComponent<Rigidbody2D>();
            temporaryprojectilebody.velocity = VectorToPlayer.normalized * BulletVelocity;
            Destroy(temporaryprojectile, 5f);
            yield return new WaitForSeconds(TimeBetweenFiring);
        }
        IsShootingCoroutineRunning = false;
    }

}
