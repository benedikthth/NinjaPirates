﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

	public PlayerOptions po;

	/// <summary>
	/// Player score displays.
	/// </summary>
	public Text player1ScoreText, player2ScoreText;
	/// <summary>
	/// game clock.
	/// </summary>
	public Text clock;
	/// <summary>
	/// Reference to the canvas object that displays the winner
	/// </summary>
	public GameObject gameOverScreen;
	/// <summary>
	/// the textobject declaring either player the winner!
	/// </summary>
	public Text winner;
	/// <summary>
	/// reference to the HUD object.
	/// </summary>
	public GameObject HUD;

	/// <summary>
	/// player scores
	/// </summary>	
	public int player1Score, player2Score;
	/// <summary>
	/// the current game duration
	/// </summary>
	public float gameTime;
	/// <summary>
	/// is the game over.  
	/// </summary>
	public bool gameOver;
	/// <summary>
	/// reference to the pause menu.
	/// </summary>
	public GameObject pauseMenu;

	/// <summary>
	/// Update player score.
	/// </summary>
	/// <param name="playerName">String containing the name of player that scored</param>
	
	public void Score(string playerName){

		Debug.Log("player: " + playerName + " Scored");

		if(playerName.Equals("Player1")){
			player1Score += 1;
		} else if(playerName.Equals("Player2")){
			player2Score += 1;
		} else {
			Debug.Log("OOPS INVALID PLAYER IN GAMEMANAGER:SCORE");
		}
	}

	// Use this for initialization
	void Start () {

		po = GameObject.Find("GameOptions").GetComponent<PlayerOptions>();
		gameOver = false;
		player1Score = 0;
		player2Score = 0;
		gameTime = po.gameDuration;
	}
	
	public void replay(){

		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void mainMenu(){
		Time.timeScale = 1;
		if(po != null){
			Destroy(po.gameObject);
		}
		SceneManager.LoadScene("Main Menu");
	}

	void endGame(){
		HUD.SetActive(false);
		gameOverScreen.SetActive(true);
		if(player1Score == player2Score){
			winner.text = "Tie!";
		} else {
			winner.text = (player1Score > player2Score)? "Player 1 wins!": "Player 2 wins!"; 
		}
		
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = (pauseMenu.activeSelf)?1:0;
			pauseMenu.SetActive(!pauseMenu.activeSelf);
		}

		gameTime -= Time.deltaTime;

		if(gameTime <= 0){
			gameOver = true;
			endGame();
		} 


		player1ScoreText.text = "Player 1 : " + player1Score;

		player2ScoreText.text = "Player 2 : " + player2Score;

		float sec = gameTime % 60;
		float min = (gameTime - sec) / 60;

		string minutes = (min < 10) ? "0" + min : min.ToString();
		string seconds = (sec - (sec % 1)).ToString();
		if(seconds.Length == 1){
			seconds = "0"+seconds;
		}
		clock.text = minutes + ":" + seconds;

		//update some shit.

	}
}
