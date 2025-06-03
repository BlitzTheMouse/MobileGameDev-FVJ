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

        //pauseMenu.SetActive(paused);

        if (paused)
        {
            SlideMenuIn(pauseMenu);
        }
        else
        {
            SlideMenuOut(pauseMenu);
        }

        onScreenControls.SetActive(!paused);
    }

    void Start()
    {
        SetPauseMenu(false);
    }
    public void SlideMenuIn(GameObject obj)
    {
        obj.SetActive(true);
        var rt = obj.GetComponent<RectTransform>();
        if (rt)
        {
            var pos = rt.position;
            pos.x = -Screen.width / 2;
            rt.position = pos;

            var tween = LeanTween.moveX(rt, 0, 1.5f);
            tween.setEase(LeanTweenType.easeInOutExpo);
            tween.setIgnoreTimeScale(true);
        }
    }

    public void SlideMenuOut(GameObject obj)
    {
        var rt = obj.GetComponent<RectTransform>();
        if (rt)
        {
            var tween = LeanTween.moveX(rt, Screen.width / 2, 0.5f);
            tween.setEase(LeanTweenType.easeOutQuad);
            tween.setIgnoreTimeScale(true);

            tween.setOnComplete(() =>
            {
                obj.SetActive(false);
            });
        }
    }

}
