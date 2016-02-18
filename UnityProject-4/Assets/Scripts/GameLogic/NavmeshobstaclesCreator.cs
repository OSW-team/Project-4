using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavmeshobstaclesCreator : MonoBehaviour
{
    public List<GameObject> Obstacles; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.B))
	    {
	        MakeObstacles();
	    }
	}

    void MakeObstacles()
    {
        foreach (var o in Obstacles)
        {
            var obstacle = o.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.shape = NavMeshObstacleShape.Box;
        }
    }
}
