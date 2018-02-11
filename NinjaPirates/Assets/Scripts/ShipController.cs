using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	// Use this for initialization
	void Start () {

		PlayerOptions playerOptions = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();

		PolygonCollider2D[] lc = GetComponents<PolygonCollider2D>();
		foreach(PolygonCollider2D e in lc){
			playerOptions.groundColliders.Add(e);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
