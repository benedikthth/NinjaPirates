using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnProvider : MonoBehaviour {

	public PlayerOptions po;

	// Use this for initialization
	void Start () {
		po = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();
		Transform[] tfs = GetComponentsInChildren<Transform>();
		po.playerRespawns = new Vector3[tfs.Length];
		int i = 0;
		foreach(Transform t in tfs){
			po.playerRespawns[i] = t.position;
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
