using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	/// <summary>
	/// This is the object that holds all the options the players share.
	/// </summary>
	public PlayerOptions po;
	/// <summary>
	/// This is the object that keeps the game Loop and score under control.
	/// </summary>
	public GameManager gm;
	/// <summary>
	/// colliders that detect if you successfully kicked the other player.
	/// </summary>
	public Collider2D leftKickCollider, rightKickController;
	/// <summary>
	/// your body. get kicked there and you go flying
	/// </summary>
	public Collider2D bodyCollider;
	/// <summary>
	/// a reference to the other players' controller.
	/// </summary>
	public Collider2D OpponentBodyCollider;
	/// <summary>
	/// a reference to your opponents script
	/// </summary>
	public PlayerController opponent;
	public Vector2 velocity;
	/// <summary>
	/// the current number of in-air jumps you have
	/// </summary>	
	int currentAirJumps;

	/// <summary>
	/// reference to own rigidbody
	/// </summary>
	public Rigidbody2D rb;
	/// <summary>
	/// Which keys control your dude.
	/// </summary>
	public KeyCode left, right, action;

	/// <summary>
	/// status booleans
	/// </summary>
	public bool stunned, canKick;

	// Use this for initialization
	void Start () {
		currentAirJumps = po.airJumps;
		stunned = false;
		canKick = true;
	}
	
	/// <summary>
	/// This gets called once the other player manages to kick you.
	/// </summary>
	/// <param name="opponentPosition">Vector2 other players position</param>
	public void kickEvent(Vector2 opponentPosition){

		Debug.Log("shit me, "+name+", got kicked");

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

	/// <summary>
	/// Reset the number of air-jumps this player has left.
	/// </summary>
	/// <param name="coll">Collision event.</param>
	void OnCollisionEnter2D(Collision2D coll){
	
		if( po.groundCollider.Contains(coll.collider) ){

			currentAirJumps = po.airJumps;
		
		}

	}
	
	/// <summary>
	/// JUMP!
	/// </summary>
	void jump(){
		if(currentAirJumps > 0){
			currentAirJumps --;
			rb.AddForce(new Vector2(0, po.jumpHeight));
		}
	}

	/// <summary>
	/// notify the playerOptions that this player has scored.
	/// </summary>
	public void Score(){
		gm.SendMessage("Score", this.name);
	}

	/// <summary>
	/// get this player off-screen for some time.
	/// </summary>
	/// <returns></returns>
	IEnumerator resetPlayer(){
		Debug.Log("resetualizing player");
		//place player away and don't simulate.
		this.transform.position = new Vector3(0, 100, 0);
		rb.bodyType = RigidbodyType2D.Static;
		//wait for the respawn timer to pass.
		yield return new WaitForSeconds(po.reviveTimer);
		//place player randomly on map and simulate.
		this.transform.position = po.playerRespawns[Random.Range(0, po.playerRespawns.Length-1)];
		rb.bodyType = RigidbodyType2D.Dynamic;

	}

	// Update is called once per frame
	void Update () {
		
		velocity = rb.velocity;

		if(this.transform.position.y < po.waterLevel){
			StartCoroutine("resetPlayer");
			opponent.SendMessage("Score");
		}

		// inputs shouldn't register if the player is stunned
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
					} else {
						Debug.Log("me, " + name + " Cant kick");
					}
				
				}
				// the player can't kick. check if he can jump instead.
				else {

					jump();
					
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
