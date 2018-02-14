using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenuManager : MonoBehaviour {
	
	public Slider durationSlider;

	public Text gameDurationHud;

    private AsyncOperation async;

    public string gameSceneName;


    IEnumerator LoadScene(string scene)
    {
        if (scene == "")
            yield break;
        Scene asyncScene = SceneManager.GetSceneByName(scene);
        async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        Debug.Log("start loading");

        while (!async.isDone)
        {
            yield return null;
        }

        async = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!async.isDone)
        {
            yield return null;
        }

        List<GameObject> list = new List<GameObject>(SceneManager.GetActiveScene().GetRootGameObjects());
        Game game = list.FirstOrDefault(o => o.name == "Manager").GetComponentInChildren<Game>();
        game.SetDuration((int)durationSlider.value * 60);
        Destroy(gameObject);
    }

    public void playGame(){
        string defaultGameScene = "BjornGame";
        StartCoroutine(LoadScene(gameSceneName != string.Empty ? gameSceneName : defaultGameScene));
    }

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        SliderChange();
    }
	
	public void SliderChange () {
		gameDurationHud.text = "Game Length: " + durationSlider.value + " minutes";
	}
}
