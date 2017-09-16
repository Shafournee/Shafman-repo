using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : BaseEnemy {

    Rigidbody2D Rigidbody;
    float VerticalVelocity;
    float HorizontalVelocity;
    float InitialDirection;
    float ChangeDirection;
    float ActualVelocity;
    enum Direction { Up, Down, Left, Right }
    List<Direction> DirectionList;


	// Use this for initialization
	void Start () {

        Rigidbody = GetComponent<Rigidbody2D>();
        ActualVelocity = 5f;
        EnemyHealth = 4;
        DirectionList = new List<Direction>();
        ChooseDirection();
    }
	
	// Update is called once per frame
	void Update () {
        Rigidbody.velocity = new Vector2(HorizontalVelocity, VerticalVelocity);
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        ChooseDirection();

        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
    }

    private void FillTheList()
    {
        DirectionList.Add(Direction.Up);
        DirectionList.Add(Direction.Down);
        DirectionList.Add(Direction.Left);
        DirectionList.Add(Direction.Right);
        ChooseDirection();
    }

    private void ChooseDirection()
    {
        if(DirectionList.Count == 0)
        {
            FillTheList();
        }
        else
        {
            int DirectionIndex = Random.Range(0, DirectionList.Count);
            ChangeWalkDirection(DirectionList[DirectionIndex]);
            DirectionList.RemoveAt(DirectionIndex);

        }
    }

    private void ChangeWalkDirection(Direction DirectionChange)
    {
        if (DirectionChange == Direction.Up)
        {
            VerticalVelocity = ActualVelocity;
            HorizontalVelocity = 0f;
        }
        else if (DirectionChange == Direction.Down)
        {
            VerticalVelocity = - ActualVelocity;
            HorizontalVelocity = 0f;
        }
        else if (DirectionChange == Direction.Right)
        {
            VerticalVelocity = 0f;
            HorizontalVelocity = ActualVelocity;
        }
        else if (DirectionChange == Direction.Left)
        {
            VerticalVelocity = 0f;
            HorizontalVelocity = - ActualVelocity;
        }
    }

    private void ChangeWalkingDirection()
    {
        float ChangeDirection = Random.Range(0f, 1f);
        if (ChangeDirection >= .5f)
        {
            //Choose right or left for velocity
            float RandomNumber;
            RandomNumber = Random.Range(0f, 1f);
            if (RandomNumber >= .5f)
            {
                VerticalVelocity = 0f;
                HorizontalVelocity = -ActualVelocity;
            }
            else
            {
                VerticalVelocity = 0f;
                HorizontalVelocity = ActualVelocity;
            }
        }
        else
        {
            //Choose up or down for velocity
            float RandomNumber;
            RandomNumber = Random.Range(0f, 1f);
            if (RandomNumber >= .5f)
            {
                VerticalVelocity = ActualVelocity;
                HorizontalVelocity = 0f;
            }
            else
            {
                VerticalVelocity = -ActualVelocity;
                HorizontalVelocity = 0f;
            }
        }
    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }

}
