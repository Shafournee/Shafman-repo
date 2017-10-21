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
    public GameObject TopCollider;
    public GameObject BottomCollider;
    public GameObject LeftCollider;
    public GameObject RightCollider;
    KeyCode D;

    // Use this for initialization
    void Start () {

        Rigidbody = GetComponent<Rigidbody2D>();
        ActualVelocity = 5f;
        EnemyHealth = 4;
        DirectionList = new List<Direction>();
        PopulateDirectionList();
        D = KeyCode.D;
    }
	
	// Update is called once per frame
	void Update () {
        Rigidbody.velocity = new Vector2(HorizontalVelocity, VerticalVelocity);
        if(HorizontalVelocity == 0 && VerticalVelocity == 0)
        {
            PopulateDirectionList();
        }
        if(Input.GetKeyDown(D))
        {
            Debug.Log(DirectionList.Count);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponent<Player>().OnHit(transform.position);
        }
        PopulateDirectionList();
    }

    private void PopulateDirectionList()
    {
        if (TopCollider.GetComponent<WalkingEnemyDirectionDetector>().IsTriggered == false)
        {
            DirectionList.Add(Direction.Up);
        }
        if (BottomCollider.GetComponent<WalkingEnemyDirectionDetector>().IsTriggered == false)
        {
            DirectionList.Add(Direction.Down);
        }
        if (RightCollider.GetComponent<WalkingEnemyDirectionDetector>().IsTriggered == false)
        {
            DirectionList.Add(Direction.Right);
        }
        if (LeftCollider.GetComponent<WalkingEnemyDirectionDetector>().IsTriggered == false)
        {
            DirectionList.Add(Direction.Left);
        }

        ChooseDirectionFromList();
    }

    private void ChooseDirectionFromList()
    {
        int DirectionIndex = Random.Range(0, DirectionList.Count);
        ChangeWalkDirection(DirectionList[DirectionIndex]);
        
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
            VerticalVelocity = -ActualVelocity;
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
            HorizontalVelocity = -ActualVelocity;
        }
        //Remove each option from the list
        DirectionList.Clear();
    }

    public override void OnHit(Vector3 DamageSourcePosition, float PlayerBulletDamage)
    {
        base.OnHit(DamageSourcePosition, PlayerBulletDamage);
    }

}
