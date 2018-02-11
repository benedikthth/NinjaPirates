using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	
	/// <summary>
	/// Player Options.
	/// </summary>	
	public PlayerOptions po;	
	public Slider durationSlider;

	public Text gameDurationHud;

	public void playGame(){
		po.setDuration((int)durationSlider.value);
		SceneManager.LoadScene("Game");
	}

	// Use this for initialization
	void Start () {
		po = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();
	}
	
	// Update is called once per frame
	void Update () {
		gameDurationHud.text = "Game Length: " + durationSlider.value + " minutes";
	}
}
