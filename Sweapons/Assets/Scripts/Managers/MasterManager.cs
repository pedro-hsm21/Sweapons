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

                gameSettings = _allgs[0];
            }

            return gameSettings;
        }
    }
}
