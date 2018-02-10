using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	public Text player1ScoreText, player2ScoreText;

	public Text clock;

	/// <summary>
	/// player scores
	/// </summary>	
	public int player1Score, player2Score;
	/// <summary>
	/// Length of game.
	/// </summary>
	public float gameLength;
	/// <summary>
	/// the current game duration
	/// </summary>
	public float gameTime;

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
		player1Score = 0;
		player2Score = 0;
		gameTime = gameLength;
	}
	
	// Update is called once per frame
	void Update () {
		gameTime -= Time.deltaTime;

		if(gameTime >= gameLength){
			//stop the game.
		} 

		player1ScoreText.text = "Player 1 : " + player1Score;

		player2ScoreText.text = "Player 2 : " + player2Score;

		float sec = gameTime % 60;
		float min = (gameTime - sec) / 60;

		string minutes = (min < 10)? "0"+min : min.ToString();
		string seconds = (sec - (sec % 1)).ToString();
		clock.text = minutes + ":" + seconds;

		//update some shit.

	}
}
