using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviourPunCallbacks
{
    public string sceneToLoad;
    
    public void LoadSoloGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public IEnumerator LoadSceneTransition()
    { 
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(sceneToLoad);
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = 1});
    }
    
    public override void OnJoinedRoom()
    {
        StartCoroutine(LoadSceneTransition());
    }
}
