using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_worldGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;
    [SerializeField]
    private Vector2 cellsize;
    private WorldTile[] cells;
    [SerializeField]
    private GameObject cellprehab;
 
    private void Awake()
    {
        cells = new WorldTile[size.x + size.y];
    }

    // Update is called once per frame
    void Update()
    {
        Generateworld();
    }

    private void Generateworld()
    {
        for(int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                GameObject newcell=Instantiate(cellprehab);
                WorldTile tile = newcell.GetComponent<WorldTile>();
                tile = cells[x + y * size.x];
            }          
        }
    }
}
