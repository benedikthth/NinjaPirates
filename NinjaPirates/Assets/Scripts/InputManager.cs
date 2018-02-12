using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    //Singleton
    private static InputManager instance = null;
    public static InputManager Instance { get { return instance; } }

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

    public KeyCode pause;
    public List<playerControl> playerControl;

    private void Update()
    {
        if (!Game.Instance.GameOver && Input.GetKeyDown(pause))
        {
            Game.Instance.PauseGame();
            //todo toggle pause ui
        }
        if (!Game.Instance.Paused && !Game.Instance.GameOver)
        {
            for (int i = 0; i < playerControl.Count; i++)
            {
                if (Input.GetKey(playerControl[i].left))
                {
                    Game.Instance.Player[i].Left();
                }
                if (Input.GetKeyDown(playerControl[i].action))
                {
                    Game.Instance.Player[i].Action();
                }
                if (Input.GetKey(playerControl[i].right))
                {
                    Game.Instance.Player[i].Right();
                }
            }
        }
    }
}

[System.Serializable]
public struct playerControl
{
    public KeyCode left, action, right;
}
