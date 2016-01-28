using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CitadelSpawner : MonoBehaviour {
    public TestController Controller;
    private SteamCitadel _playersCitadel;
    private SteamCitadel _enemyCitadel;
    public GameObject PlayersCitadel;
    public GameObject EnemyCitadel;
    public GameObject PlayerSpawn;
    public GameObject EnemySpawn;
    public Transform PlayerSpawnArea;
    public Transform EnemySpawnArea;
    // Use this for initialization
    void Start () {
        var master = FindObjectOfType<MasterMindNHWheels>();
        _playersCitadel = Controller.MyCitadel;
        _enemyCitadel = new SteamCitadel("EnemySC1");
        XMLWorker.LoadSC(_enemyCitadel);
        PlayersCitadel = SteamCitadelMeshConstrutor.BuildCitadelMesh(_playersCitadel);
        EnemyCitadel =  SteamCitadelMeshConstrutor.BuildCitadelMesh(_enemyCitadel);
        PlayersCitadel.transform.position = PlayerSpawn.transform.position;
        PlayersCitadel.transform.localScale = PlayerSpawn.transform.localScale;
        EnemyCitadel.transform.position = EnemySpawn.transform.position;
        EnemyCitadel.transform.localScale = EnemySpawn.transform.localScale;

        var playerUnitGOs = new List<GameObject>();
        foreach (var unit in _playersCitadel.Units)
        {
            unit.BuildMesh();
            playerUnitGOs.Add(unit.GO);
        }

        var enemyUnitGOs = new List<GameObject>();
        foreach (var unit in _enemyCitadel.Units)
        {
            unit.BuildMesh();
            enemyUnitGOs.Add(unit.GO);
        }
        var playerSpawnUnit = PlayersCitadel.AddComponent<SpawnUnit>();
            playerSpawnUnit.Units = playerUnitGOs.ToArray();
            playerSpawnUnit.SpawnPoint = PlayerSpawn.transform;
            playerSpawnUnit.Master = master;
            playerSpawnUnit.PointDown = PlayerSpawn.transform;
            playerSpawnUnit.PointMarch = EnemySpawn.transform;
            playerSpawnUnit.EnemySF = EnemyCitadel;
            playerSpawnUnit.SpawnArea = PlayerSpawnArea;
            playerSpawnUnit.Team = 0;

        var enemySpawnUnit = EnemyCitadel.AddComponent<SpawnUnit>();
            enemySpawnUnit.Units = enemyUnitGOs.ToArray();
            enemySpawnUnit.SpawnPoint = EnemySpawn.transform;
            enemySpawnUnit.Master = master;
            enemySpawnUnit.PointDown = EnemySpawn.transform;
            enemySpawnUnit.PointMarch = PlayerSpawn.transform;
            enemySpawnUnit.EnemySF = PlayersCitadel;
            enemySpawnUnit.SpawnArea = EnemySpawnArea;
            enemySpawnUnit.Team = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
