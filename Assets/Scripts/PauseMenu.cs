using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

    private NetworkManager networkManager; 

    //Called by pause button and X button
    public void TogglePauseMenu() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    //Called by button to leave the game
    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost(); 
    }
}
