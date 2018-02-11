using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public PlayerOptions po;	
	public Slider durationSlider;


	public void playGame(){
		Time.timeScale = 1;
		po.setDuration((int)durationSlider.value);
		SceneManager.LoadScene("Game");
	}

	// Use this for initialization
	void Start () {
		po = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
