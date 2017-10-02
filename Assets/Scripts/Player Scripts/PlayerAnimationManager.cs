using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyDirection { Up, Down, Left, Right, Static }
public enum HeadDirection { Up, Down, Left, Right, Static }

public class PlayerAnimationManager : MonoBehaviour {

    //Get the animators
    Animator HeadAnimator;
    Animator BodyAnimator;
    SpriteRenderer SpriteHead;
    public Sprite LookingUp;
    public Sprite LookingDown;
    public Sprite LookingLeft;
    public Sprite LookingRight;
    public Sprite ShootingDown;


    // Use this for initialization
    void Start () {

        HeadAnimator = transform.Find("Head").GetComponent<Animator>();
        BodyAnimator = transform.Find("Body").GetComponent<Animator>();
        SpriteHead = transform.Find("Head").GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}







    //Handles the direction and animations of the head
    public void HeadAnimationManager(HeadDirection ShootDirection)
    {
        if (ShootDirection == HeadDirection.Down)
        {
            SpriteHead.sprite = LookingDown;
        }
        else if (ShootDirection == HeadDirection.Up)
        {
            SpriteHead.sprite = LookingUp;
        }
        else if (ShootDirection == HeadDirection.Left)
        {
            SpriteHead.sprite = LookingLeft;
        }
        else if (ShootDirection == HeadDirection.Right)
        {
            SpriteHead.sprite = LookingRight;
        }
    }

    private IEnumerator HeadAnimation(HeadDirection ShootDirection)
    {
        if(ShootDirection == HeadDirection.Down)
        {
            SpriteHead.sprite = ShootingDown;
            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
            SpriteHead.sprite = LookingDown;
        }
            yield return null;
    }

    //Handles the direction and animations of the body
    public void BodyAnimationManager(BodyDirection MoveDirection)
    {
        if (MoveDirection == BodyDirection.Down)
        {
            BodyAnimator.Play("WalkDown");
        }
        else if (MoveDirection == BodyDirection.Up)
        {
            BodyAnimator.Play("WalkUp");
        }
        else if (MoveDirection == BodyDirection.Left)
        {
            BodyAnimator.Play("WalkLeft");
        }
        else if (MoveDirection == BodyDirection.Right)
        {
            BodyAnimator.Play("WalkRight");
        }
        else if (MoveDirection == BodyDirection.Static)
        {
            BodyAnimator.Play("Standing");
        }
    }
}
