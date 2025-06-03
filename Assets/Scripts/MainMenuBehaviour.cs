using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuBehaviour : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highscoretext;

    int highscore;

    public float score = 0;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


    public void ScoreDelete()
    {
        PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        highscoretext.text = PlayerPrefs.GetInt("score").ToString();
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