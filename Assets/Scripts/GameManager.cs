using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static bool gamePaused;
    GameObject menu;
    Canvas pauseCanvas;
    int currentLevel;



    void Awake ()
    {
        menu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseCanvas = menu.GetComponent<Canvas>();
        currentLevel = Application.loadedLevel;
        gamePaused = false;
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Pause"))
        {
            gamePaused = !gamePaused;
        }

	}

    void OnGUI ()
    {
        if (gamePaused)
        {
            pauseCanvas.enabled = true;
            Time.timeScale = (0);
        }

        if (!gamePaused)
        {
            pauseCanvas.enabled = false;
            Time.timeScale = (1);
        }
    }

    public void StartClick ()
    {
        Application.LoadLevel(1);
    }

    public void RestartClick ()
    {
        Application.LoadLevel(currentLevel);
    }

    public void QuitClick ()
    {
        Application.Quit();
    }
}
