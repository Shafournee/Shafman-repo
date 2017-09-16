using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totes : BaseBoss {

    Rigidbody2D Rigidbody;
    bool RageActive;
    int SpinSpeed;
    public GameObject TotesBody;
    public GameObject Attack;
    public GameObject Rest;
    public GameObject HalfHealth;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        BossMaxHealth = 10f;
        BossCurrentHealth = 10f;
        SpinSpeed = 360;
        DoesDamageOnHit = true;
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(BeginAttack());
        RageActive = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (BossCurrentHealth / BossMaxHealth <= .5f)
        {
            RageActive = true;
            Rest.SetActive(false);
            Attack.SetActive(false);
            HalfHealth.SetActive(true);
        }

    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    IEnumerator BeginAttack()
    {
        int ActiveSpinSpeed;
        if (RageActive)
        {
            ActiveSpinSpeed = 1000;
        }
        else
        {
            ActiveSpinSpeed = SpinSpeed;
            Rest.SetActive(false);
            Attack.SetActive(true);
        }
        for(int i = 0; i < 60; i++)
        {
            TotesBody.transform.Rotate(Vector3.forward, Time.deltaTime * ActiveSpinSpeed, Space.World);
            yield return null;
        }
        StartCoroutine(SpinAttack(ActiveSpinSpeed));
    }

    private IEnumerator SpinAttack(int SpinAttackSpeed)
    {
        float DirectionalVelocity;
        if (RageActive)
        {
            DirectionalVelocity = 25f;
        }
        else
        {
            DirectionalVelocity = 15f;
        }
        float StartDirectionx = Mathf.Sign(Random.Range(-1, 1));
        float StartDirectiony = Mathf.Sign(Random.Range(-1, 1));
        Vector2 UnitVectorDirection = new Vector2(.45f * StartDirectionx, .45f * StartDirectiony).normalized;
        Rigidbody.velocity = UnitVectorDirection * DirectionalVelocity;
        for (int i = 0; i < 180; i++)
        {
            TotesBody.transform.Rotate(Vector3.forward, Time.deltaTime * SpinAttackSpeed, Space.World);
            yield return null;
        }
        SpinRecovery();
    }

    private void SpinRecovery()
    {
        Rigidbody.velocity = new Vector3(0f, 0f, 1f);
        StartCoroutine(TimeBeforeNextSpin());
    }

    private IEnumerator TimeBeforeNextSpin()
    {
        float WaitForNextAttack;
        if (RageActive)
        {
            WaitForNextAttack = 0f;
        }
        else
        {
            Rest.SetActive(true);
            Attack.SetActive(false);
            WaitForNextAttack = Random.Range(1f, 2f);
        }
        yield return new WaitForSeconds(WaitForNextAttack);
        StartCoroutine(BeginAttack());
    }
    

}
