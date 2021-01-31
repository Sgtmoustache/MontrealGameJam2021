using System.Collections;
using Cinemachine;
using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(PlayerSpawner))]
public class GameManager : MonoBehaviourPun
{
    public static GameManager _Instance;
    private static int TeacherScore = 0;
    private static int StudentScore = 0;
    public int CurrentRound = 0;
    [HideInInspector] public int NumberOfStudentDetention = 0;
    [HideInInspector] public int NumberOfFalseAccusation = 0;
    
    public static bool PlayersCanMove = false;
    public static bool PlayersSpawned = false;
    public static Transform CameraPosition = null;
    public static int TeacherViewID = 0;
    
    [Header("Debug")]
    [SerializeField] public Transform BotDebugTargetOverwrite = null;
    [SerializeField] private bool SkipIntro = false;
    
    [Header("Score settings")]
    [SerializeField] private int FalseAccusationMultiplier = 20;
    [SerializeField] private int DetentionMultiplier = 10;
    [SerializeField] private int LostAndFoundScore = 100;
    [SerializeField] private int OutsideScore = 100;
    [SerializeField] private int HiddenScore = 10;
    
    [Header("Settings")]
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private CinemachineVirtualCamera Camera;
    [SerializeField] private Transform endZonePosition;
    
    [Header("Timer")]
    [SerializeField] private int startBufferDuration;
    [SerializeField] private int[] roundDuration;
    [SerializeField] private int scoreBoardDuration;
    [SerializeField] private int winnerScreenDuration;
    [SerializeField] private int bufferBetweenRounds;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI roundLabel;
    [SerializeField] private TextMeshProUGUI winnerLabel;
    [SerializeField] private TextMeshProUGUI teacherScoreLabel;
    [SerializeField] private TextMeshProUGUI studentScoreLabel;
    [SerializeField] private GameObject winnerBoard;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private GameObject gameUI;

    private PlayerSpawner _playerSpawner;
    [HideInInspector] public ItemManager ItemManager;


    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;
    
    private PlayerMovement PlayerMovement;
    void Start()
    {
        _Instance = this;
        _playerSpawner = GetComponent<PlayerSpawner>();
        _playerSpawner.camera = Camera;
        ItemManager = GetComponent<ItemManager>();
        CameraPosition = CameraTransform.transform;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("TestRoom");
        }
        
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(StartGame());
    }

    public void BroadcastFalseAccusation()
    {
        photonView.RPC("ReportFalseAccusation", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void ReportFalseAccusation()
    {
        FalseAccusationMultiplier++;
    }

    public void BroadcastDetention()
    {
        photonView.RPC("ReportDetention", RpcTarget.MasterClient);
    }
    
    [PunRPC]
    public void ReportDetention()
    {
        NumberOfStudentDetention++;
    }

    [PunRPC]
    private void SetViewIDTeacher(int value)
    {
        Debug.LogWarning($"Teacher is {value}");
        TeacherViewID = value;
    }
    
    [PunRPC]
    private void SetPlayerCanMove(bool value)
    {
        Debug.LogWarning(value ? "Unlocking players" : "Locking players");
        PlayersCanMove = value;
    }

    [PunRPC]
    private void PlayIntroAnimation()
    {
        Debug.LogWarning("Playing intro animation");
    }
    
    [PunRPC]
    private void SetGameUILablelVisibility(bool visibility)
    {
        Debug.LogWarning($"Setting {gameUI.name} visibility to {visibility.ToString()}");
        gameUI.SetActive(visibility);
    }
    
    [PunRPC]
    private void SetRoundUILabel(string value)
    {
        Debug.LogWarning($"Setting {roundLabel.name} value to {value}");
        roundLabel.text = value;
    }
    
    [PunRPC]
    private void SetTeacherScoreUILabel(string value)
    {
        Debug.LogWarning($"Setting {teacherScoreLabel.name} value to {value}");
        teacherScoreLabel.text = value;
    }
    
    [PunRPC]
    private void SetStudentScoreUILabel(string value)
    {
        Debug.LogWarning($"Setting {studentScoreLabel.name} value to {value}");
        studentScoreLabel.text = value;
    }
    
    [PunRPC]
    private void SetScoreboardVisibility(bool value)
    {
        Debug.LogWarning($"Setting {scoreBoard.name} visibility to {value}");
        scoreBoard.SetActive(value);
    }
    
    [PunRPC]
    private void SetWinnerVisibility(bool value)
    {
        Debug.LogWarning($"Setting {winnerBoard.name} visibility to {value}");
        winnerBoard.SetActive(value);
    }
    
    [PunRPC]
    private void SetWinnerUILabel(string value)
    {
        Debug.LogWarning($"Setting {winnerLabel.name} value to {value}");
        winnerLabel.text = value;
    }
    
    [PunRPC]
    private void ActivateSeeTroughtHandlers(bool value)
    {
        Debug.LogWarning("Activating seethroughthandlers");
        PlayersSpawned = value;
    }

    [PunRPC]
    void SyncCurrentRoundCount(int value)
    {
        Debug.LogWarning("Sync current round");
        CurrentRound = value;
    }

    private IEnumerator StartGame()
    {
        Debug.LogWarning("Starting game");
        Debug.LogWarning($"Waiting start buffer for {startBufferDuration} seconds");
        yield return new WaitForSeconds(startBufferDuration);

        if (!SkipIntro)
        {    
            yield return FadeManager._Instance.FadeInRoutine();
            //Start intro animation
            photonView.RPC("PlayIntroAnimation", RpcTarget.All);
            yield return new WaitForSeconds(5);
            yield return FadeManager._Instance.FadeOutRoutine();
        }

        yield return new WaitForSeconds(bufferBetweenRounds);
        photonView.RPC("SetViewIDTeacher", RpcTarget.All, Random.Range(1, PhotonNetwork.CountOfPlayers));
        
        _playerSpawner.SpawnPlayers();
        _playerSpawner.SpawnBots();
        
        photonView.RPC("ActivateSeeTroughtHandlers", RpcTarget.All, true);

        for (CurrentRound = 0; CurrentRound < roundDuration.Length; CurrentRound++)
        {
            photonView.RPC("SyncCurrentRoundCount", RpcTarget.Others, CurrentRound);
            //Start Round
            Debug.LogWarning($"Starting round {CurrentRound + 1}/{roundDuration.Length} for {roundDuration[CurrentRound]} seconds");
            //Round UI refresh
            photonView.RPC("SetRoundUILabel", RpcTarget.All, $"Round {CurrentRound + 1}/{roundDuration.Length}" );
            yield return StartRound(roundDuration[CurrentRound]);
            //Show score
            CalculateScore();
            yield return ShowScoreboard();
            yield return new WaitForSeconds(bufferBetweenRounds);
        }
        
        photonView.RPC("RespawnPlayer", RpcTarget.All, endZonePosition.position);
        photonView.RPC("SetPlayerCanMove", RpcTarget.All, true);
        yield return FadeManager._Instance.FadeInRoutine();

        yield return ShowWinner();
        
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private IEnumerator StartRound(int duration)
    {
        ItemManager.RefreshItems(CurrentRound != 0);
        photonView.RPC("RespawnPlayer", RpcTarget.All, Vector3.zero);
        photonView.RPC("SetGameUILablelVisibility", RpcTarget.All, true);
        photonView.RPC("SetPlayerCanMove", RpcTarget.All, true);
        yield return FadeManager._Instance.FadeInRoutine();
        yield return new WaitForSeconds(duration);
        photonView.RPC("SetPlayerCanMove", RpcTarget.All, false);
        photonView.RPC("SetGameUILablelVisibility", RpcTarget.All,false);    
    }

    private void CalculateScore()
    {
        //Calculating teacher score
        foreach (var holder in ItemManager.OutsidePlaceHolders)
        {
            if (holder.HasItem())
            {
                TeacherScore += OutsideScore;
            }
        }
        
        TeacherScore += NumberOfStudentDetention * DetentionMultiplier;
        TeacherScore -= NumberOfFalseAccusation * FalseAccusationMultiplier;
        
        //Calculating student score
        foreach (var holder in ItemManager.HiddenSpotPlaceHolders)
        {
            if (holder.HasItem())
            {
                StudentScore += HiddenScore;
            }
        }
        
        foreach (var holder in ItemManager.LostAndFoundPlaceHolders)
        {
            if (holder.HasItem())
            {
                StudentScore += LostAndFoundScore;
            }
        }
    }

    private IEnumerator ShowScoreboard()
    {
        Debug.LogWarning($"Showing scoreboard for {scoreBoardDuration} seconds");
        photonView.RPC("SetTeacherScoreUILabel", RpcTarget.All, TeacherScore.ToString() );
        photonView.RPC("SetStudentScoreUILabel", RpcTarget.All, StudentScore.ToString() );
        photonView.RPC("SetScoreboardVisibility", RpcTarget.All, true);
        yield return new WaitForSeconds(scoreBoardDuration/2);
        yield return FadeManager._Instance.FadeOutRoutine();
        yield return new WaitForSeconds(scoreBoardDuration/2);
        photonView.RPC("SetScoreboardVisibility", RpcTarget.All, false);
    }

    private IEnumerator ShowWinner()
    {
        Debug.LogWarning($"Showing winner for {winnerScreenDuration}");
        string result;
        
        if (StudentScore == TeacherScore)
            result = "DRAW";
        else if (StudentScore > TeacherScore)
            result = "STUDENTS WIN";
        else
            result = "TEACHER WINS";
        
        photonView.RPC("SetWinnerUILabel", RpcTarget.All, result);
        photonView.RPC("SetWinnerVisibility", RpcTarget.All, true);
        yield return new WaitForSeconds(winnerScreenDuration);
        yield return FadeManager._Instance.FadeOutRoutine();
        photonView.RPC("SetWinnerVisibility", RpcTarget.All, false);
    }
}
