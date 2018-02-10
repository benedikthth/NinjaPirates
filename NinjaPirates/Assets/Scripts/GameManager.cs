using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

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
	
	public void replay(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void mainMenu(){

	}

	void endGame(){

		gameOverScreen.SetActive(true);
		if(player1Score == player2Score){
			winner.text = "Tie!";
		} else {
			winner.text = (player1Score > player2Score)? "Player 1 wins!": "Player 2 wins!"; 
		}	
	}

	// Update is called once per frame
	void Update () {
		gameTime -= Time.deltaTime;

		if(gameTime >= gameLength){
			Time.timeScale = 0;
			endGame();
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
