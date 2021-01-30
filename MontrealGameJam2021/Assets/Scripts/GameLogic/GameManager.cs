using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviourPun
{
    private static int TeacherScore = 0;
    private static int StudentScore = 0;
    private static int currentRound = 0;

    [SerializeField] private int startBufferDuration;
    [SerializeField] private int[] roundDuration;
    [SerializeField] private int scoreBoardDuration;
    [SerializeField] private int winnerScreenDuration;
    [SerializeField] private int bufferBetweenRounds;
    [SerializeField] private GameObject winnerBoard;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private TextMeshProUGUI roundLabel;
    [SerializeField] private TextMeshProUGUI winnerLabel;
    [SerializeField] private TextMeshProUGUI teacherScoreLabel;
    [SerializeField] private TextMeshProUGUI studentScoreLabel;

    [SerializeField] private PlayerSpawner PlayerSpawner;

    [SerializeField] private Transform endzonePosition;

    private PlayerMovement PlayerMovement;
    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(startBufferDuration);
        //Start intro animation
        Debug.LogWarning($"Playing intro animation for 5 seconds");
        yield return new WaitForSeconds(5);
        yield return FadeManager._Instance.FadeOutRoutine();
        yield return new WaitForSeconds(bufferBetweenRounds);
        
        PlayerSpawner.SpawnPlayers();
        PlayerSpawner.SpawnBots();
        
        PlayerMovement = PlayerSpawner.LocalPlayer.GetComponent<PlayerMovement>();
        
        for (currentRound = 0; currentRound < roundDuration.Length; currentRound++)
        {
            //Start Round
            Debug.LogWarning($"Starting round {currentRound + 1}/{roundDuration.Length} for {roundDuration[currentRound]} seconds");
            roundLabel.text = $"Round {currentRound + 1}/{roundDuration.Length}";
            yield return StartRound(roundDuration[currentRound]);
            //Show score
            yield return ShowScoreboard();
            yield return new WaitForSeconds(bufferBetweenRounds);
        }

        PlayerSpawner.RespawnPlayer(endzonePosition);
        yield return FadeManager._Instance.FadeInRoutine();

        yield return ShowWinner();
        
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private IEnumerator StartRound(int duration)
    {
        PlayerSpawner.RespawnPlayer();        
        gameUI.SetActive(true);
        PlayerMovement.CanMove = true;
        yield return FadeManager._Instance.FadeInRoutine();
        yield return new WaitForSeconds(duration);
        PlayerMovement.CanMove = false;
        gameUI.SetActive(false);
    }

    private IEnumerator ShowScoreboard()
    {
        Debug.LogWarning($"Showing scoreboard for {scoreBoardDuration} seconds");
        teacherScoreLabel.text = TeacherScore.ToString();
        studentScoreLabel.text = StudentScore.ToString();
        scoreBoard.SetActive(true);
        yield return new WaitForSeconds(scoreBoardDuration/2);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(scoreBoardDuration/2);
        scoreBoard.SetActive(false);
    }

    private IEnumerator ShowWinner()
    {
        Debug.LogWarning($"Showing winner for {winnerScreenDuration}");
        if (StudentScore == TeacherScore)
            winnerLabel.text = "DRAW";
        else if (StudentScore > TeacherScore)
            winnerLabel.text = "STUDENTS WIN";
        else
            winnerLabel.text = "TEACHER WINS";
        
        winnerBoard.SetActive(true);
        yield return new WaitForSeconds(winnerScreenDuration);
        yield return FadeManager._Instance.FadeOutRoutine();
        winnerBoard.SetActive(false);
    }
}
