using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "MyAssets/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] string gameVersion = "0.0.1";
    public string GameVersion => gameVersion;

    [SerializeField] string nickName = "DDPedro";
    public string NickName
    {
        get
        {
            int id = Random.Range(0, 999999);
            return nickName + id;
        }
    }
}
