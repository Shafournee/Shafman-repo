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
    public Sprite ShootingUp;
    public Sprite ShootingDown;
    public Sprite ShootingLeft;
    public Sprite ShootingRight;
    float ShootingSpeed;


    // Use this for initialization
    void Start () {

        HeadAnimator = transform.Find("Head").GetComponent<Animator>();
        BodyAnimator = transform.Find("Body").GetComponent<Animator>();
        SpriteHead = transform.Find("Head").GetComponent<SpriteRenderer>();
        ShootingSpeed = gameObject.GetComponent<Player>().MinimumTimeBetweenFiring + 7;
    }
	
	// Update is called once per frame
	void Update () {
		
	}







    //Handles the direction and animations of the head
    public void HeadAnimationManager(HeadDirection ShootDirection)
    {
        SpriteHead.flipX = false;
        StartCoroutine(HeadAnimation(ShootDirection));
    }

    private IEnumerator HeadAnimation(HeadDirection ShootDirection)
    {
        if(ShootDirection == HeadDirection.Down)
        {
            SpriteHead.sprite = ShootingDown;
            for (int i = 0; i < ShootingSpeed; i++)
            {
                yield return null;
            }
            SpriteHead.sprite = LookingDown;
            yield return null;
        }

        if (ShootDirection == HeadDirection.Up)
        {
            SpriteHead.sprite = ShootingUp;
            for (int i = 0; i < ShootingSpeed; i++)
            {
                yield return null;
            }
            SpriteHead.sprite = LookingUp;
            yield return null;
        }

        if (ShootDirection == HeadDirection.Left)
        {
            SpriteHead.flipX = true;
            SpriteHead.sprite = ShootingLeft;
            for (int i = 0; i < ShootingSpeed; i++)
            {
                yield return null;
            }
            SpriteHead.sprite = LookingLeft;
            yield return null;
        }

        if (ShootDirection == HeadDirection.Right)
        {
            SpriteHead.sprite = ShootingRight;
            for (int i = 0; i < ShootingSpeed; i++)
            {
                yield return null;
            }
            SpriteHead.sprite = LookingRight;
            yield return null;
        }

        else if(ShootDirection == HeadDirection.Static)
        {
            SpriteHead.sprite = LookingDown;
        }
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
