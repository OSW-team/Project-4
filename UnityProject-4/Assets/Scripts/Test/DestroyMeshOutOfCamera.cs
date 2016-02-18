using UnityEngine;
using System.Collections;

public class DestroyMeshOutOfCamera : MonoBehaviour
{
    private Renderer _renderer;
    private bool check = true;
    private MeshFilter[] _meshFilters;
    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (_renderer.isVisible)
        {
            if (check)
            {
                _meshFilters = GetComponentsInChildren<MeshFilter>();
                Debug.Log("Visible");
                check = !check;
            }

        }
        else {
            if (!check)
            {
                Debug.Log("Invisible");
                for (var i = 0; i < _meshFilters.Length; i++)
                {
                    _meshFilters[i].gameObject.SetActive(false);
                }
                check = !check;
            }
        }
    }
}
