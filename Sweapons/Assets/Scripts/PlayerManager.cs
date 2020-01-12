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
                if (_instance == null)
                    _instance = FindObjectOfType<PlayerManager>();

                if (_instance == null)
                    _instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return _instance;
        }
    }

    [SerializeField] int playerCount = 2;
    [SerializeField] Color[] playerColors;

    [SerializeField] int initialPlayerWeapon = 0;
    [SerializeField] Weapon[] weaponsPrefab;
    [SerializeField] Player playerPrefab;

    int[] weaponsPlayerNumbers;
    Player[] players;

    GameObject[] spawnPoints;

    IEnumerator ItemSpawner;
    [SerializeField] float spawnFrequency = 3.0f;
    [SerializeField] [Range(0, 1)] float spawnChance = 0.75f;
    [SerializeField] GameObject[] collectablesPrefab;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        SpawnAllPlayers();
    }

    private void Start()
    {
        StartCoroutine(ItemSpawnCoroutine());
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

        List<int> allSpawnPositionIndexes = GetAllIndexesIn(spawnPoints.Length);
        List<int> allPlayerIndexes = GetAllIndexesIn(playerCount);
        weaponsPlayerNumbers = new int[playerCount];
        players = new Player[playerCount];

        for (int i = 0; i < playerCount; i++)
        {            
            int spawnPositionIndex = GetRandomIndexAndRemoveItFromList(ref allSpawnPositionIndexes);
            Vector3 playerPosition = spawnPoints[spawnPositionIndex].transform.position;
            players[i] = Instantiate(playerPrefab, playerPosition, Quaternion.identity) as Player;
            players[i].SetPlayerNumber(i, playerColors[i]);

            int _weaponPlayerNumber = GetRandomIndexAndRemoveItFromList(ref allPlayerIndexes, i);
            weaponsPlayerNumbers[i] = _weaponPlayerNumber;
            SetWeaponToPlayer(i, initialPlayerWeapon);
        }
    }

    public void SetWeaponToPlayer (int _playerNumber, int weaponIndex)
    {
        Weapon _weapon = Instantiate(weaponsPrefab[weaponIndex]) as Weapon;
        _weapon.SetPlayerNumber(weaponsPlayerNumbers[_playerNumber], playerColors[weaponsPlayerNumbers[_playerNumber]]);
        players[_playerNumber].SetWeapon(_weapon.gameObject);
    }

    public void StopPlayersBehavior()
    {
        for (int i = 0; i < playerCount; i++)
        {
            players[i].StopPlayerMovement();
        }
    }

    public void StartPlayersBehavior()
    {
        for (int i = 0; i < playerCount; i++)
        {
            players[i].StartPlayerMovement();
        }
    }

    public List<int> GetAllIndexesIn(int range)
    {
        List<int> allPositions = new List<int>();
        for (int i = 0; i < range; i++)
        {
            allPositions.Add(i);
        }
        return allPositions;
    }

    public int GetRandomIndexAndRemoveItFromList(ref List<int> indexList, int restriction = -1)
    {
        if (restriction != -1) indexList.Remove(restriction);

        int randomIndex = Random.Range(0, indexList.Count);
        int spawnPositionIndex = indexList[randomIndex];
        indexList.RemoveAt(randomIndex);

        if (restriction != -1) indexList.Add(restriction);
        return spawnPositionIndex;
    }

    public IEnumerator ItemSpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            float rng = Random.Range(0.0f, 1.0f);
            //print("the rng was: " + rng);
            if (rng < spawnChance)
            {
                GameObject _go = collectablesPrefab[Random.Range(0, collectablesPrefab.Length)];
                Instantiate(_go, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            }
        }
    }

    public bool CanPlayerMove(int playerNumber)
    {
        return players[playerNumber].IsAlive && players[playerNumber].CanMove;
    }

    public bool EndGame ()
    {
        return players[0].PlayerLifes() <= 0 || players[1].PlayerLifes() <= 0;
    }
}
