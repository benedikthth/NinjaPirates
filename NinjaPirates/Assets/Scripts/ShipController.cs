using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	public PlayerOptions po;
	// Use this for initialization
	void Start () {
		PlayerOptions playerOptions = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();
		EdgeCollider2D[] lc = GetComponents<EdgeCollider2D>();
		foreach(EdgeCollider2D e in lc){
			playerOptions.groundCollider.Add(e);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
