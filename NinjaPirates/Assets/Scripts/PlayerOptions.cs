using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerOptions : MonoBehaviour {

	public PlayerOptions po;

	/// <summary>
	/// list of colliders that count as the ground.
	/// </summary>
	public List<Collider2D> groundColliders;

	/// <summary>
	/// How long one game should be.
	/// </summary>
	public int gameDuration = 120;
	/// <summary>
	/// how hard a player should kick.
	/// </summary>
	public float kickForce;
	/// <summary>
	/// how long in seconds the player should be stunned if kicked.
	/// </summary>
	public float stunDuration;
	/// <summary>
	/// how many 'double-jumps' a player should have.
	/// </summary>
	public int airJumps;
	/// <summary>
	/// How fast a player should run.
	/// </summary>
	public float runSpeed;
	/// <summary>
   	/// how high a player should jump.
   	/// </summary>
	public float jumpHeight;
	/// <summary>
	/// how long until a player should wait in between kicks.
	/// </summary>
	public float kickCoolDown;
	/// <summary>
	/// At which level the player should die and respawn.
	/// </summary>
	public float waterLevel = -3.95f;

	/// <summary>
	/// How long the player should wait until he's revived.
	/// </summary>
	public float reviveTimer;

	public Vector3[] playerRespawns;


	public void setDuration(int x){
		this.po.gameDuration = 60 * x;
	}


	void Awake(){
		Singleton();
	}

	void Singleton(){
         if(po == null){
             DontDestroyOnLoad(gameObject);
             po = this;
         }
         else{
             if(po != this){
                 Destroy (gameObject);
             }
         }
		}

	// Use this for initialization
	void Start () {
		/* 
		if(GameObject.Find("GameOptions") != null)
		{
			Destroy(this);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
