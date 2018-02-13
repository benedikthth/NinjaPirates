using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    //Singleton
    private static Game instance = null;
    public static Game Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //how many players to spawn
    public int playerCount;
    //list of player colors, applied in the same order as players.
    public List<Color> playerColor;

    //reference to the playerPrefab, and gameParent used to initilize playerPrefab
    public GameObject playerPrefab;
    public GameObject gameParent;

    //a list of all the player classes 
    private List<Player> player = new List<Player>();
    public List<Player> Player { get { return player; } }

    public GameObject ship;

    //a bool to see if the game is paused
    private bool paused;
    public bool Paused { get { return paused; } }

    //a bool to see if the game is over
    private bool gameOver;
    public bool GameOver { get { return gameOver; } }

    //game current and maxDuration
    public int maxDuration = 120;
    private float currentDuration;

    //temporary copy of the respawn position list, (todo: move respawn positions to ship + masts.)
    public List<Vector3> tempRespawnList = new List<Vector3>();

    private void Start()
    {
        StartGame();
    }

    //a function that pauses the game, stops gameTime shows the pauseScreen;
    public void PauseGame()
    {
        paused = !paused;
        UIManager.Instance.pauseScreen.SetActive(paused);
        if (paused)
        {
            //pause
            Time.timeScale = 0;
        }
        else
        {
            //unpause
            Time.timeScale = 1;
        }
    }

    //a reference to the timerTick coroutine, used to interrupt it.
    Coroutine timer;
    //a coroutine function that ticks down the gameTime, updates the UI for the timer, and if allowed to expire, runs the endGame function.
    IEnumerator TimerTick()
    {
        float currentDuration = maxDuration;

        while (currentDuration > 0)
        {
            float sec = (currentDuration % 60f);
            float min = ((currentDuration - sec) / 60f);
            UIManager.Instance.SetTimer(((min < 10) ? "0" + min.ToString("#") : min.ToString("##")) + ":" + ((sec < 10) ? "0" + sec : sec.ToString("##")));
            yield return new WaitForSeconds(1f);
            currentDuration -= 1f;
        }
        UIManager.Instance.SetTimer("00:00");

        EndGame();
    }

    //a function that, discards, and generates new player game objects.
    private void SpawnPlayers()
    {
        for (int i = 0; i < player.Count; i++)
        {
            Destroy(player[i].gameObject);
        }
        player.Clear();
        for (int i = 0; i < playerCount; i++)
        {
            Player p = Instantiate(playerPrefab, tempRespawnList[i], Quaternion.identity, gameParent.transform).GetComponent<Player>();
            p.Color = playerColor[i];
            player.Add(p);
        }
    }

    //a function that signals the start of a new game.
    //it resets the ui (score, time), resets bool variables, starts the game timer and spawns new players.
    public void StartGame()
    {
        UIManager.Instance.ResetUI();
        paused = false;
        Time.timeScale = 1;
        gameOver = false;
        SpawnPlayers();

        if (timer != null)
        {
            StopCoroutine(timer);
        }
        timer = StartCoroutine(TimerTick());
    }


    // a function that signals the end of the game, set gameOver, stop time and show the GameOver UI
    public void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0;
        UIManager.Instance.GameOver();
    }

    public void SetDuration(int duration)
    {
        maxDuration = duration;
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        timer = StartCoroutine(TimerTick());
    }

}
