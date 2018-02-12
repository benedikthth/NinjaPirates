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

    //the keyCode for the pause
    public KeyCode pause;
    //a list of all player controls, uses same index as player list. (Game.Instance.Player)
    public List<playerControl> playerControl;

    private void Update()
    {
        //if game not over and pause is pressed.
        if (!Game.Instance.GameOver && Input.GetKeyDown(pause))
        {
            Game.Instance.PauseGame();
        }
        //if game is not over and is not paused.
        if (!Game.Instance.Paused && !Game.Instance.GameOver)
        {
            for (int i = 0; i < playerControl.Count; i++)
            {
                //if player i is pressing down his "left" button.
                if (Input.GetKey(playerControl[i].left))
                {
                    Game.Instance.Player[i].Left();
                }
                //if player i is pressing down his "action" button.
                if (Input.GetKeyDown(playerControl[i].action))
                {
                    Game.Instance.Player[i].Action();
                }
                //if player i is pressing down his "right" button.
                if (Input.GetKey(playerControl[i].right))
                {
                    Game.Instance.Player[i].Right();
                }
            }
        }
    }
}

//a struct to organize each players custom inputs.
[System.Serializable]
public struct playerControl
{
    public KeyCode left, action, right;
}
