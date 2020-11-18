﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseShade;
    private Animator animator;
    public string mainMenuSceneName;
    // animatable representation of the current time scale
    public Slider timeSlider;
    private bool canPause;

    public Transform optionsButton;
    public Transform resumeButton;
    public Transform menuButton;

    // Start is called before the first frame update
    void Start()
    {
        canPause = false;
        // deactivate the shade on game start
        pauseShade.SetActive(false);
        // setup animator
        animator = pauseShade.GetComponent<Animator>();
        // set "timeScale" to 1 at start
        timeSlider.value = 1f;

        StartCoroutine(enablePause());
    }

    IEnumerator enablePause()
    {
        yield return new WaitForSeconds(2);
        canPause = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            TogglePause();
        }
        Time.timeScale = timeSlider.value;
    }

    private void TogglePause()
    {
        if (paused)
        {
            // start exit animation
            (optionsButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartExit();
            (menuButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartExit();
            (resumeButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartExit();

            StartCoroutine(IResume());
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseShade.SetActive(true);
            paused = true;

            // start entry animation
            (optionsButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartEntry();
            (menuButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartEntry();
            (resumeButton.GetComponent(typeof(PauseMenuButtonController)) as PauseMenuButtonController).StartEntry();
        }
    }

    private IEnumerator IResume()
    {

        animator.Play("Exit");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // wait until anim stops - use real time outside of time.timeScale
        canPause = false;
        yield return StartCoroutine(IWaitForRealSeconds(0.8f));
        canPause = true;
        // reset time scale to account for rounding errors
        Time.timeScale = 1f;
        timeSlider.value = 1f;
        pauseShade.SetActive(false);
        paused = false;

    }

    public void Click_Resume()
    {
        TogglePause();
    }

    public void Click_Options()
    {
        Time.timeScale = 1f;
        // implement options menu
        string settingsSceneName = "SettingsMenu 1";
        Scene currentPausedScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += SceneLoaded;

        // deactivate audio listener
        GameObject mCam = GameObject.FindGameObjectWithTag("MainCamera");
        AudioListener aList = mCam.GetComponent<AudioListener>();
        if (aList) aList.enabled = false;

        // load settings scene
        SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
    }

    // load main menu scene (title screen)
    public void Click_MainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit to Main Menu! Scene \"" + mainMenuSceneName + "\" now loading...");
        TogglePause();
        if (!mainMenuSceneName.Equals(""))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private static IEnumerator IWaitForRealSeconds(float seconds)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + seconds)
        {
            yield return null;
        }
    }

    //this is only run when the settings scene is loaded, after running, it removes itself from SceneManager
    private void SceneLoaded(Scene s, LoadSceneMode l)
    {
        if(l == LoadSceneMode.Additive)
        {

        }

        SceneManager.sceneLoaded -= SceneLoaded;
    }
}