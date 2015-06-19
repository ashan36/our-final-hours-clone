using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof (NotificationsManager))]
public class GameManager : MonoBehaviour 
{

    public static bool gamePaused;
    GameObject menu;
    Canvas pauseCanvas;
    int currentLevel;

    public int currentPoints = 0;

    private static GameManager instance = null;

    private static NotificationsManager notifications = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>(); //create game manager object if required
            return instance;
        }
    }

    public static NotificationsManager Notifications
    {
        get
        {
            if (notifications == null) notifications = instance.GetComponent<NotificationsManager>();
            return notifications;
        }
    }



    void Awake ()
    {
        //Check if there is an existing instance of this object
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }

        menu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseCanvas = menu.GetComponent<Canvas>();
        currentLevel = Application.loadedLevel;
        gamePaused = false;

    }

	// Use this for initialization
	void Start () 
    {
        NotificationsManager.DefaultNotifier.AddObserver(this, "OnEnemyKilled");
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

    void OnEnemyKilled (NotificationsManager.Notification notification)
    {
        if (notification.EventArgs is int)
        currentPoints += (int)notification.EventArgs;
        Debug.Log("current points: " + currentPoints);
    }
}
