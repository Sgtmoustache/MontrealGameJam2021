using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] private Bounds MapBounds;
    [SerializeField] private int numberOfBots = 50;
    [HideInInspector] public CinemachineVirtualCamera camera;
    
    [SerializeField] private List<Transform> botsSpawns = new List<Transform>();
    [SerializeField] private List<Transform> studentsSpawns = new List<Transform>();
    [SerializeField] private List<Transform> teacherSpawns = new List<Transform>();
    [SerializeField] private Transform detentionEnter = new RectTransform();
    [SerializeField] private Transform detentionExit = new RectTransform();

    public static GameObject LocalPlayer;

    private bool IsTeacher => GameManager.TeacherViewID == PhotonNetwork.LocalPlayer.ActorNumber;

    
    [PunRPC]
    public void SpawnPlayers(RawImage image)
    {
        Debug.LogWarning("Spawning player");

        if(PhotonNetwork.IsMasterClient)
            photonView.RPC("SpawnPlayers", RpcTarget.Others);

        string role;
        Vector3 position;
        Quaternion rotation;
        
        if (IsTeacher)
        {
            int randomValue = Random.Range(0, teacherSpawns.Count-1);
            role = "Teacher";
            position = teacherSpawns[randomValue].position;
            rotation = teacherSpawns[randomValue].rotation;
            image.gameObject.SetActive(true);
        }
        else
        {
            int randomValue = Random.Range(0, studentsSpawns.Count-1);
            role = "Student";
            position = studentsSpawns[randomValue].position;
            rotation = studentsSpawns[randomValue].rotation;
        }
        
        if (PhotonNetwork.IsConnected)
            LocalPlayer = PhotonNetwork.Instantiate($"Prefabs/{role}", position, rotation);
        else
            LocalPlayer = (GameObject) Instantiate(Resources.Load($"Prefabs/{role}"), position, rotation);
        
        
        LocalPlayer.GetComponent<PlayerMovement>().Camera = GameManager.CameraPosition;
        LocalPlayer.gameObject.layer = 6;
        LocalPlayer.GetComponentInChildren<Light>().enabled = true;
        LocalPlayer.GetComponent<VisibilityHandler>().enabled = false;
        LocalPlayer.GetComponent<PlayerInfo>().isLocal = true;
        LocalPlayer.gameObject.tag = "Player";
        
        camera.Follow = LocalPlayer.transform;

        if (IsTeacher)
        {
            LocalPlayer.GetComponent<Attack>().detentionSpawn = detentionEnter;
            LocalPlayer.GetComponent<Attack>().detentionSpawnExit = detentionExit;
        }
    }

    [PunRPC]
    public void RespawnPlayer(Vector3 forceLocation)
    {
        if (forceLocation == Vector3.zero)
        {
            Debug.LogWarning("Respawning player");

            if (IsTeacher)
            {
                int randomValue = Random.Range(0, teacherSpawns.Count-1);

                LocalPlayer.transform.position = teacherSpawns[randomValue].position;
                LocalPlayer.transform.rotation = teacherSpawns[randomValue].rotation;
            }
            else
            {
                int randomValue = Random.Range(0, studentsSpawns.Count-1);

                LocalPlayer.transform.position = studentsSpawns[randomValue].position;
                LocalPlayer.transform.rotation = studentsSpawns[randomValue].rotation;    
            }
        }
        else
        {
            Debug.LogWarning("Going to specific location");
            LocalPlayer.transform.position = forceLocation;
        }
        
        LocalPlayer.GetComponent<Inventory>().ClearItem();
    }

    public void SpawnBots()
    {
        Debug.LogWarning("Spawning Bots");
        
        List<GameObject> bots = new List<GameObject>();
        
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add(PhotonNetwork.Instantiate("Prefabs/StudentBot",  botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }
        else if (!PhotonNetwork.IsConnected)
        {
            for (int i = 0; i < numberOfBots; i++)
            {
                int randomIndex = Random.Range(0, botsSpawns.Count);

                bots.Add((GameObject) Instantiate(Resources.Load("Prefabs/StudentBot"), botsSpawns[randomIndex].position, botsSpawns[randomIndex].rotation));
            }
        }
        
        foreach (GameObject bot in bots)
        {
            bot.GetComponent<BotMovement>()._bounds = MapBounds;
        }
    }
}