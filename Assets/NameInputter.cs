using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameInputter : MonoBehaviour
{
    public void InputName(string name) {
        PlayerData.playerName = name;
    }
}
