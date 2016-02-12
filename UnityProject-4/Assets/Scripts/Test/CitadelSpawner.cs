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
		var master = FindObjectOfType<MasterMindTranslate>();
        _playersCitadel = Controller.MyCitadel;
        _enemyCitadel = new SteamCitadel("EnemySC1");
        XMLWorker.LoadSC(_enemyCitadel);
        PlayersCitadel = SteamCitadelMeshConstrutor.BuildCitadelMesh(_playersCitadel);
        EnemyCitadel =  SteamCitadelMeshConstrutor.BuildCitadelMesh(_enemyCitadel);
        PlayersCitadel.transform.position = PlayerSpawn.transform.position;
        PlayersCitadel.transform.localScale = PlayerSpawn.transform.localScale;
		PlayersCitadel.transform.rotation = PlayerSpawn.transform.rotation;
        EnemyCitadel.transform.position = EnemySpawn.transform.position;
        EnemyCitadel.transform.localScale = EnemySpawn.transform.localScale;
		EnemyCitadel.transform.rotation = EnemySpawn.transform.rotation;
		/*
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
        }*/
        var playerSpawnUnit = PlayersCitadel.AddComponent<SpawnUnit>();
			playerSpawnUnit.Units = _playersCitadel.Units.ToArray ();
			playerSpawnUnit.SpawnPoint = _playersCitadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Constructor).GO.transform.FindChild("SpawnPoint");
            playerSpawnUnit.Master = master;
			playerSpawnUnit.PointDown = _playersCitadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Constructor).GO.transform.FindChild("DownPoint");
            playerSpawnUnit.PointMarch = EnemySpawn.transform;
            playerSpawnUnit.EnemySF = EnemyCitadel;
            playerSpawnUnit.SpawnArea = PlayerSpawnArea;
            playerSpawnUnit.Team = 0;

        var enemySpawnUnit = EnemyCitadel.AddComponent<SpawnUnit>();
			enemySpawnUnit.Units = _enemyCitadel.Units.ToArray ();
			enemySpawnUnit.SpawnPoint = _enemyCitadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Constructor).GO.transform.FindChild("SpawnPoint");
            enemySpawnUnit.Master = master;
			enemySpawnUnit.PointDown = _enemyCitadel.Modules.FirstOrDefault(x => x.Type == GameModelsAndEnums.EnumModuleType.Constructor).GO.transform.FindChild("DownPoint");
            enemySpawnUnit.PointMarch = PlayerSpawn.transform;
            enemySpawnUnit.EnemySF = PlayersCitadel;
            enemySpawnUnit.SpawnArea = EnemySpawnArea;
            enemySpawnUnit.Team = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
