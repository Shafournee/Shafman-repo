using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryDude : BaseBoss {

    public GameObject LeftTrigger;
    public GameObject RightTrigger;
    public GameObject TopTrigger;
    public GameObject BottomTrigger;

    enum MoveDirection { Left, Right, Up, Down };
    enum ChargeDirection { Left, Right, Up, Down };
    MoveDirection WayToMove;
    ChargeDirection WayToCharge;
    float MoveSpeed;
    float ChargeSpeed;
    Rigidbody2D RigidBody;
    public bool IsCharging;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        BossMaxHealth = 10f;
        BossCurrentHealth = 10f;
        DoesDamageOnHit = true;
        MoveSpeed = 3f;
        ChargeSpeed = 5f;
        RigidBody = GetComponent<Rigidbody2D>();
        IsCharging = false;
        Movement();
    }

    // Update is called once per frame
    void Update () {
        if(LeftTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true || RightTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true || TopTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true || BottomTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true)
        {
            MoveTowardsTriggeredSide();
        }
	}

    private void MoveTowardsTriggeredSide()
    {
        if (IsCharging == false)
        {
            if (LeftTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true)
            {
                WayToCharge = ChargeDirection.Left;
                RigidBody.velocity = new Vector3(-ChargeSpeed, 0f, 1f);
            }
            else if (RightTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true)
            {
                WayToCharge = ChargeDirection.Right;
                RigidBody.velocity = new Vector3(ChargeSpeed, 0f, 1f);
            }
            else if (TopTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true)
            {
                WayToCharge = ChargeDirection.Up;
                RigidBody.velocity = new Vector3(0f, ChargeSpeed, 1f);
            }
            else if (BottomTrigger.GetComponent<AngryDudeDetector>().IsTriggered == true)
            {
                WayToCharge = ChargeDirection.Down;
                RigidBody.velocity = new Vector3(0f, -ChargeSpeed, 1f);
            }
            IsCharging = true;
        }
        
    }

    IEnumerator ToggleIsCharging()
    {
        IsCharging = true;
        for(int i = 0; i < 30; i++)
        {
            yield return null;
        }
        IsCharging = false;
    }

    private void Movement()
    {
        int ChooseDirection;
        ChooseDirection = Random.Range(0, 4);

        if(ChooseDirection == 0)
        {
            RigidBody.velocity = new Vector3(-MoveSpeed, 0f, 1f);
            WayToMove = MoveDirection.Left;
        }
        else if (ChooseDirection == 1)
        {
            RigidBody.velocity = new Vector3(MoveSpeed, 0f, 1f);
            WayToMove = MoveDirection.Right;
        }
        else if (ChooseDirection == 2)
        {
            RigidBody.velocity = new Vector3(0f, MoveSpeed, 1f);
            WayToMove = MoveDirection.Up;
        }
        else if (ChooseDirection == 3)
        {
            RigidBody.velocity = new Vector3(0f, -MoveSpeed, 1f);
            WayToMove = MoveDirection.Down;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        int MoveinDifferentDirection;
        MoveinDifferentDirection = Random.Range(0, 2);

        if (WayToMove == MoveDirection.Left || WayToCharge == ChargeDirection.Left)
        {
            
            if(MoveinDifferentDirection == 0)
            {
                RigidBody.velocity = new Vector3(0f, MoveSpeed, 1f);
                WayToMove = MoveDirection.Up;
            }
            else if(MoveinDifferentDirection == 1)
            {
                RigidBody.velocity = new Vector3(0f, -MoveSpeed, 1f);
                WayToMove = MoveDirection.Down;
            }
        }
        else if (WayToMove == MoveDirection.Right || WayToCharge == ChargeDirection.Right)
        {
            if (MoveinDifferentDirection == 0)
            {
                RigidBody.velocity = new Vector3(0f, MoveSpeed, 1f);
                WayToMove = MoveDirection.Up;
            }
            else if (MoveinDifferentDirection == 1)
            {
                RigidBody.velocity = new Vector3(0f, -MoveSpeed, 1f);
                WayToMove = MoveDirection.Down;
            }
        }
        else if (WayToMove == MoveDirection.Up || WayToCharge == ChargeDirection.Up)
        {
            if (MoveinDifferentDirection == 0)
            {
                RigidBody.velocity = new Vector3(-MoveSpeed, 0f, 1f);
                WayToMove = MoveDirection.Left;
            }
            else if (MoveinDifferentDirection == 1)
            {
                RigidBody.velocity = new Vector3(MoveSpeed, 0f, 1f);
                WayToMove = MoveDirection.Right;
            }
        }
        else if (WayToMove == MoveDirection.Down || WayToCharge == ChargeDirection.Down)
        {
            if (MoveinDifferentDirection == 0)
            {
                RigidBody.velocity = new Vector3(-MoveSpeed, 0f, 1f);
                WayToMove = MoveDirection.Left;
            }
            else if (MoveinDifferentDirection == 1)
            {
                RigidBody.velocity = new Vector3(MoveSpeed, 0f, 1f);
                WayToMove = MoveDirection.Right;
            }
        }
        IsCharging = false;
    }
    
    //CHECK IF THE POSITION HAVEN'T MOVED AND IF THEY HAVEN'T TAKE THE OTHER CHOICE

}
