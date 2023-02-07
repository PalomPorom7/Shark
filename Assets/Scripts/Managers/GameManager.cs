using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
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

    private void Start()
    {
        StartGame();
    }
    private void StartGame()
    {
        activeColors = startActiveColors;
        StartCoroutine(ChangeSharkEyeColor());
        StartCoroutine(AddColor());
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
        gameOver = true;
        StopAllCoroutines();
        print("Game Over");
    }
}
