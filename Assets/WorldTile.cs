using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour
{
    [SerializeField]
    private Vector2 size;
    Vector2Int coordinates;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public WorldTile(int x, int y)
    {
        transform.position = new Vector3(size.x*x, 0, size.y * y);
    }
}
