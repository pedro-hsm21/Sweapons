using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : SingletonMonoBehavior<MasterManager>
{
    [SerializeField] GameSettings gameSettings;
    public GameSettings ActiveGameSettings
    {
        get
        {
            if (gameSettings == null)
            {
                GameSettings[] _allgs = Resources.FindObjectsOfTypeAll<GameSettings>();

                if (_allgs.Length > 0)
                    gameSettings = _allgs[0];
                else
                    Debug.LogError("Não foi possível carregar um GameSetting");
            }

            return gameSettings;
        }
    }
}
