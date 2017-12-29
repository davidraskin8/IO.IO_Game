using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    private NetworkManager networkManager;

    // Use this for initialization
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            //if the matchmaker does not exist start it
            networkManager.StartMatchMaker();
        }

        status.text = "Loading...";
        RefreshRoomList();
    }
     
    public void RefreshRoomList()
    {
        ClearRoomList();
        status.text = "Loading...";
        //get list of matches from matchmaker
        networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        
    }


    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        status.text = "";

        if (success)
        {
             
            //find the match with the minimum number of players and join that match
            foreach (MatchInfoSnapshot match in matches)
            {
                GameObject roomListItemGO = Instantiate(roomListItemPrefab, roomListParent);

                RoomListItem _roomListItem = roomListItemGO.GetComponent<RoomListItem>();
                if(_roomListItem != null)
                {
                    _roomListItem.SetMatchInfo(match, JoinRoom);
                }
                //have component sit on game object that will set name/amount of users
                //as wel as setting up callback function that will join the game.

                roomList.Add(roomListItemGO); 
            }

            Debug.Log(roomList.Count);
            if(roomList.Count == 0)
            {
                status.text = "No Rooms"; 
            }
        }
        else
        {
            Debug.Log("Couldn't Connect to Matchmaker");
        } 
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom (MatchInfoSnapshot _match )
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining...";
    }


}

