using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static int level = 1;
    public bool paused = true;
    public int score = 0, circlesPerWave, targetScore;
    public GameObject circlePrefab, panel, background, pauseButton;
    public Text scoreText, levelText, timerText;
    public Sprite[] backgroundSprites;
    float time = 0;
    bool isTimerOn = false;
    Color colorOfLevel;

    private void Start()
    {
        targetScore *= level;
        Time.timeScale = 1f;
        colorOfLevel = Random.ColorHSV();
        background.GetComponent<SpriteRenderer>().sprite = backgroundSprites[Random.Range(0,3)];
        levelText.text = "Level: " + level;
    }

    private void FixedUpdate()
    {
        if (isTimerOn) time += Time.deltaTime;
        timerText.text = "" + Mathf.RoundToInt(time);
        
        //FOR PC
        if(Input.GetMouseButton(0))
        {
            transform.GetChild(0).transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
        //FOR MOBILE
        /*
        if (Input.touchCount > 0)
        {
            transform.GetChild(0).transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }*/
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        score += Mathf.RoundToInt(1 / (collision.GetComponent<CircleScript>().speed) * 10);
        if(score >= targetScore && paused)
        {
            level++;
            levelText.gameObject.SetActive(true);
            levelText.text = "Level: " + level;
            panel.SetActive(true);
            Time.timeScale = 0f;
        }
        scoreText.text = "Your Score: " + score;
        Destroy(collision.gameObject);
    }

    public void StartButton()
    {
        isTimerOn = true;
        pauseButton.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(false);
    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 1f:0f;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void SpawnCircles()
    {
        for (int i = 0; i < circlesPerWave + level; i++)
        {
            float sizeOfCircle = Random.Range(0.1f, 1f);
            GameObject curCircle = Instantiate(circlePrefab, new Vector2(Random.Range(-2, 3), 10), Quaternion.identity, transform);
            curCircle.transform.localScale = new Vector2(sizeOfCircle, sizeOfCircle);
            curCircle.GetComponent<SpriteRenderer>().color = colorOfLevel;
            curCircle.GetComponent<CircleScript>().speed *= level / sizeOfCircle;
        }
        Invoke("SpawnCircles", 1f);
    }
}