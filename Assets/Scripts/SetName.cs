using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SetName : NetworkBehaviour{
    [SyncVar(hook = "OnChangeName")]
    public string currentName;

    public Text nameText;

    //Change healthbar GUI when a player takes damage
    void OnChangeName(string currentName)
    {
        nameText.text = currentName;
    }

    public void SetText(string name)
    {
        currentName = name;
    }
}
