using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	/// <summary>
	/// This is the object that holds all the options the players share.
	/// </summary>
	public PlayerOptions po;
	public Collider2D leftKickCollider, rightKickController;
	public Collider2D bodyCollider;

	public Collider2D OpponentBodyCollider;

	public PlayerController opponent;
	public Vector2 velocity;
	
	int currentAirJumps;
	public Rigidbody2D rb;
	
	public KeyCode left, right, action;

	
	public bool stunned, canKick;

	// Use this for initialization
	void Start () {
		currentAirJumps = po.airJumps;
		stunned = false;
	}
	
	/// <summary>
	/// This gets called once the other player manages to kick you.
	/// </summary>
	/// <param name="opponentPosition">Vector2 other players position</param>
	public void kickEvent(Vector2 opponentPosition){
		
		Vector2 kickDirection = (Vector2)this.transform.position - opponentPosition;

		rb.AddForce(kickDirection * po.kickForce);

		stunned = true;
		StartCoroutine("Stunned");
	
	}

	/// <summary>
	/// Start cooldown until you are not stunned anymore.
	/// </summary>
	/// <returns></returns>
	IEnumerator Stunned() {
    
		yield return new WaitForSeconds(po.stunDuration);

		stunned = false;
	}

	/// <summary>
	/// Start the cooldown to be able to kick again.
	/// </summary>
	/// <returns></returns>
	IEnumerator KickCoolDown(){
		yield return new WaitForSeconds(po.kickCoolDown);

		canKick = true;
	}

	// Update is called once per frame
	void Update () {
		
		velocity = rb.velocity;


		// inputs
		if(!stunned){

			// the action key is the kick/jump key. 	
			if(Input.GetKeyDown(action)){


				//prioritize kicks over jumping.
				if(leftKickCollider.IsTouching(OpponentBodyCollider) || rightKickController.IsTouching(OpponentBodyCollider)){
					
					//TODO: decide that if you cant kick, you should jump instead.
					if(canKick){
						canKick = false;
						StartCoroutine("KickCoolDown");
						opponent.SendMessage("kickEvent", (Vector2)this.transform.position);
					}
				
				}
				// the player can't kick. check if he can jump instead.
				else {

					if(bodyCollider.IsTouching(po.groundCollider)){
						//reset airJumps to max extra jumps.
						currentAirJumps = po.airJumps;
						//jump!
						rb.AddForce(new Vector2(0, po.jumpHeight));

					}  
					//check if the player still has airJumps left.
					else if (currentAirJumps > 0){
						//spend air jump
						currentAirJumps --;
						//jump!
						rb.AddForce(new Vector2(0, po.jumpHeight));
					} 
				}
			}

			//
			if(Input.GetKey(left)){
				if(this.velocity.x < po.runSpeed){
					rb.AddForce(new Vector2(-po.runSpeed, 0));
				}
			} 

			if(Input.GetKey(right)){	
				if(this.velocity.x < po.runSpeed){
					rb.AddForce(new Vector2(po.runSpeed, 0));
				}
			} 
			// stop the player from running.
			//if(Input.GetKeyUp(right) || Input.GetKey(left)){
			//	rb.velocity = new Vector2(0, rb.velocity.y);
			//}



		}
	}
}
