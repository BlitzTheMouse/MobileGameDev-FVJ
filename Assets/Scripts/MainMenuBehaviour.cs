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
}