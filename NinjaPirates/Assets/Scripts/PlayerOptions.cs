using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptions : MonoBehaviour {

	public Collider2D groundCollider;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
