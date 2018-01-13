using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDudeOfFlies : BaseBoss {

    Rigidbody2D Rigidbody;
    public GameObject SmallFlyPrefab;
    public GameObject BigFlyPrefab;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        BossMaxHealth = 40f;
        BossCurrentHealth = 40f;
        DoesDamageOnHit = true;
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        ChangeStartDirection();
        StartCoroutine(SpawnTheFlies());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeStartDirection()
    {
        float StartDirectionx = Mathf.Sign(Random.Range(-1, 1));
        float StartDirectiony = Mathf.Sign(Random.Range(-1, 1));
        Vector2 UnitVectorDirection = new Vector2(.45f * StartDirectionx, .45f * StartDirectiony).normalized;
        Rigidbody.velocity = UnitVectorDirection * 3f;
    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private IEnumerator SpawnTheFlies()
    {
        while(true)
        {
            //Determines how long it takes to spawn flies and how many
            int SecondsToWaitForFlySpawn = Random.Range(3, 8);
            int RandomNumberOfSpawnFlies = Random.Range(1, 4);
            for (int i = 0; i < RandomNumberOfSpawnFlies; i++)
            {
                //Determines which fly spawns with small flys having a 70% chance
                GameObject InstantiatedFly;
                float WhichFly = Random.Range(0f, 1f);
                float FlyStartDirectionx = Mathf.Sign(Random.Range(-1, 1));
                float FlyStartDirectiony = Mathf.Sign(Random.Range(-1, 1));
                if (WhichFly < .7f)
                {
                    InstantiatedFly = SmallFlyPrefab;
                }
                else
                {
                    InstantiatedFly = BigFlyPrefab;
                }
                GameObject SpawnFly = Instantiate(InstantiatedFly, new Vector3(gameObject.transform.position.x + 1.5f * FlyStartDirectionx, gameObject.transform.position.y + 1.5f * FlyStartDirectiony, 1f), gameObject.transform.rotation).gameObject;
            }
            yield return new WaitForSeconds(SecondsToWaitForFlySpawn);
        }
    }

}
