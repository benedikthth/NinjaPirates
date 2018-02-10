using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public Collider2D leftKickCollider, rightKickController;
	public Collider2D bodyCollider;

	public Collider2D OpponentBodyCollider;

	public PlayerController opponent;
	public Vector2 velocity;
	public Collider2D groundCollider;

	public int airJumps;
	int currentAirJumps;
	public Rigidbody2D rb;
	public float runSpeed, jumpHeight;

	public KeyCode left, right, action;

	// Use this for initialization
	void Start () {
		currentAirJumps = airJumps;
	}
	
	public void kickEvent(){
		
	}

	// Update is called once per frame
	void Update () {
		
		velocity = rb.velocity;

		if(Input.GetKeyDown(action)){
			if(bodyCollider.IsTouching(groundCollider)){
				
				currentAirJumps = airJumps;
				rb.AddForce(new Vector2(0, jumpHeight));

			}  else if (currentAirJumps > 0){
				currentAirJumps --;
				rb.AddForce(new Vector2(0, jumpHeight));
			}else {

				if(Input.GetKey(left)){
					//check if leftKickCollider is touching the other players body collider
					if(leftKickCollider.IsTouching(OpponentBodyCollider)){
						opponent.SendMessage("kickEvent");
					}
				}
				if(Input.GetKey(right)){
					//check if leftKickCollider is touching the other players body collider
					if(rightKickController.IsTouching(OpponentBodyCollider)){
						opponent.SendMessage("kickEvent");
					}
				}

			}
		}

		if(Input.GetKey(left)){
			if(this.velocity.x < runSpeed){
				rb.AddForce(new Vector2(-runSpeed, 0));
			}
		} 

		if(Input.GetKey(right)){	
			if(this.velocity.x < runSpeed){
				rb.AddForce(new Vector2(runSpeed, 0));
			}
		} 
		
		if(Input.GetKeyUp(right) || Input.GetKey(left)){
			rb.velocity.Set(0, rb.velocity.y);
		}



	}
}
