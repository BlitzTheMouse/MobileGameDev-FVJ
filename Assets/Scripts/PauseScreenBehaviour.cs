using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehaviour : MonoBehaviour
{
    public static bool paused;

    [Tooltip("Reference to the pause menu object to turn on / off")]
    public GameObject pauseMenu;

    [Tooltip("Reference to the on screen controls menu")]
    public GameObject onScreenControls;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;
    }

    public void SetPauseMenu(bool isPaused)
    {
        paused = isPaused;

        Time.timeScale = (paused) ? 0 : 1;

        pauseMenu.SetActive(paused);

        onScreenControls.SetActive(!paused);
    }

    void Start()
    {
        SetPauseMenu(false);
    }
}
