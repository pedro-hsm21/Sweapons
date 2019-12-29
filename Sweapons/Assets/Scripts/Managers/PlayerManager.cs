using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject _go = new GameObject("InputManager");
                _instance = _go.AddComponent<PlayerManager>() as PlayerManager;
            }

            return _instance;
        }
    }

    [SerializeField] int playerCount = 2;
    [SerializeField] Color[] playerColors;

    [SerializeField] Weapon[] weapons;
    [SerializeField] Player playerPrefab;

    int[] weaponPlayer;
    Player[] players;

    GameObject[] spawnPoints;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private void Start()
    {
        SpawnAllPlayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            SceneManager.LoadScene("MainGame");
    }

    public void SetSpawnPosition(Transform _transform)
    {
        _transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    void SpawnAllPlayers()
    {
        weaponPlayer = new int[playerCount];
        players = new Player[playerCount];
        List<int> allPositions = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            allPositions.Add(i);
        }

        for (int i = 0; i < playerCount; i++)
        {
            int instanciationPosition = allPositions[Random.Range(0, allPositions.Count)];
            allPositions.Remove(instanciationPosition);

            players[i] = Instantiate(playerPrefab, spawnPoints[instanciationPosition].transform.position, Quaternion.identity) as Player;
            players[i].SetPlayerNumber(i, playerColors[i]);

            int _weaponPlayerNumber = i == 0 ? 1 : 0;
            Weapon _weapon = Instantiate(weapons[0]) as Weapon;
            _weapon.SetPlayerNumber(_weaponPlayerNumber, playerColors[_weaponPlayerNumber]);
            players[i].SetWeapon(_weapon.gameObject);
        }
    }
}
