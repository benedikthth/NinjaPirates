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

    public int playerCount;
    public List<Color> playerColor;

    public GameObject playerPrefab;
    public GameObject gameParent;

    private List<Player> player = new List<Player>();
    public List<Player> Player { get { return player; } }

    private bool paused;
    public bool Paused { get { return paused; } }
    private bool gameOver;
    public bool GameOver { get { return gameOver; } }

    public int maxDuration = 120;
    private float currentDuration;

    public List<Vector3> tempRespawnList = new List<Vector3>();

    private void Start()
    {
        StartGame();
    }

    public void PauseGame()
    {
        paused = !paused;
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

    Coroutine timer;
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

    public void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0;
        UIManager.Instance.GameOver();
    }

}
