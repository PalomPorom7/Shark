using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject gameOverScreen,
                        instructionsScreen;

    [SerializeField]
    private InputManager input;

    [SerializeField]
    private Shark player;

    [SerializeField]
    private List<Color> colors;
    [SerializeField]
    private int startActiveColors = 2;
    private int activeColors;

    [SerializeField]
    private Text scoreUI;
    private int score = 0;

    private bool gameOver;

    [SerializeField]
    private Text gameOverScore;

    private void Start()
    {
        input.enabled = false;
    }
    public void Reset()
    {
        StartCoroutine(FishSpawner.Instance.Fishpocalypse());
        score = 0;
        scoreUI.text = "Score : 0";
        gameOverScreen.SetActive(false);
        gameOver = false;
        player.Reset();
        StartGame();
    }
    public void StartGame()
    {
        instructionsScreen.SetActive(false);
        activeColors = startActiveColors;
        StartCoroutine(ChangeSharkEyeColor());
        StartCoroutine(AddColor());
        input.enabled = true;
    }
    public Color GetRandomColor(bool colorMustBeActive = false)
    {

        return colors[Random.Range(0, colorMustBeActive ? activeColors : colors.Count)];
    }
    private IEnumerator AddColor()
    {
        while(!gameOver)
        {
            yield return new WaitForSeconds(60);
            ++activeColors;
        }
    }
    private IEnumerator ChangeSharkEyeColor()
    {
        while(!gameOver)
        {
            yield return StartCoroutine(player.ChangeEyeColor(GetRandomColor(true)));
            yield return new WaitForSeconds(10);
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreUI.text = "Score : " + score;
    }
    public void GameOver()
    {
        input.enabled = false;
        gameOverScore.text = "Score : " + score;
        gameOverScreen.SetActive(true);
        gameOver = true;
        StopAllCoroutines();
        print("Game Over");
    }
}
