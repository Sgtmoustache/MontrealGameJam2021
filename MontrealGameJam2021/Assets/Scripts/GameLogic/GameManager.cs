using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(PlayerSpawner))]
public class GameManager : MonoBehaviourPun
{
    public static int TeacherScore = 0;
    public static int StudentScore = 0;
    private int CurrentRound = 0;

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

    private PlayerSpawner _playerSpawner;
    private ItemManager _itemManager;

    [SerializeField] private Transform endZonePosition;

    private PlayerMovement PlayerMovement;
    void Start()
    {
        _playerSpawner = GetComponent<PlayerSpawner>();
        _itemManager = GetComponent<ItemManager>();
        
        //TODO IF HOST
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
        
        _playerSpawner.SpawnPlayers();
        _playerSpawner.SpawnBots();
        
        PlayerMovement = PlayerSpawner.LocalPlayer.GetComponent<PlayerMovement>();
        
        for (CurrentRound = 0; CurrentRound < roundDuration.Length; CurrentRound++)
        {
            //Start Round
            Debug.LogWarning($"Starting round {CurrentRound + 1}/{roundDuration.Length} for {roundDuration[CurrentRound]} seconds");
            roundLabel.text = $"Round {CurrentRound + 1}/{roundDuration.Length}";
            yield return StartRound(roundDuration[CurrentRound]);
            //Show score
            yield return ShowScoreboard();
            yield return new WaitForSeconds(bufferBetweenRounds);
        }

        _playerSpawner.RespawnPlayer(endZonePosition);
        yield return FadeManager._Instance.FadeInRoutine();

        yield return ShowWinner();
        
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private IEnumerator StartRound(int duration)
    {
        _itemManager.RefreshItems();
        _playerSpawner.RespawnPlayer();        
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
