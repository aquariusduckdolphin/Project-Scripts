using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/Sensitivity", order = 2)]
public class PlayerInfo : ScriptableObject
{

    [Range(500f, 2000f)]
    public float mouseSensitivity = 1000f;

}
