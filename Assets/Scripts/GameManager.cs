using System.Collections;
using System.Collections.Generic; //List
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tiles to spawn with no obstacles")]
    public int initNoObstacles = 4;

    public Vector3 nextTileLocation;
    private Quaternion nextTileRotation;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highscoretext;

    int highscore;

    public float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;
        for (int i = 0; i < initSpawnNum; ++i)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }

    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
        var nextTile = newTile.Find("NextSpawnPoint");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    private void SpawnObstacle(Transform newTile)
    {
        var obstacleSpawnPoints = new List<GameObject>();

        foreach (Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }
        if (obstacleSpawnPoints.Count > 0)
        {
            int index = Random.Range(0,obstacleSpawnPoints.Count);
            var spawnPoint = obstacleSpawnPoints[index];
            var spawnPos = spawnPoint.transform.position;
            var newObstacle = Instantiate(obstacle,spawnPos, Quaternion.identity);
            newObstacle.SetParent(spawnPoint.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * 5;
        highscore = (int)score;
        scoreText.text = highscore.ToString();

        if (PlayerPrefs.GetInt("score") <= highscore)
        {
            PlayerPrefs.SetInt("score", highscore);
        }
        highscoretext.text = PlayerPrefs.GetInt("score").ToString();

    }
    /*public void highscorefun()
    {

        highscoretext.text = PlayerPrefs.GetInt("score").ToString();
    }*/

    public float Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (scoreText == null)
            {
                Debug.LogError("Score Text is not set. " + "Please go to the Inspector and assign it");
                return;
            }
            scoreText.text = string.Format("{0:0}", score);

            int formattedScore = Mathf.FloorToInt(score);

            if (formattedScore > PlayerPrefs.GetInt("score", 0))
            {
                PlayerPrefs.SetInt("score", formattedScore);
            }
        }
    }

    public void ScoreDelete()
    {
        PlayerPrefs.DeleteAll();
    }
}
