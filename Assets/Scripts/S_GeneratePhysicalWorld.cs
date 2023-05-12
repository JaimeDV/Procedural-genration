using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class S_GeneratePhysicalWorld : MonoBehaviour
{
    private Node[,] starGrid;

    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private GameObject ground;

    /// <summary>
    /// using the A*grid generates the physical world
    /// </summary>
  
    private void GeneratePhysicalWordl()
    {
        foreach (var node in starGrid)
        {
            if (node != null)
            {
                if (node.isNotWall)
                {
                   Instantiate(ground, node.position, gameObject.transform.rotation);
          
                }
                else
                {
                    Instantiate(wall, node.position, gameObject.transform.rotation);
                }
            }
        }
        
    }
    private void reciveGrid(Node[,] grid)
    {
        starGrid = grid;
    }
    private void OnEnable()
    {
        Grid.GetGrid += reciveGrid;
        WorldGenerator.createWorld += GeneratePhysicalWordl;
    }
    private void OnDisable()
    {
        Grid.GetGrid -= reciveGrid;
        WorldGenerator.createWorld -= GeneratePhysicalWordl;
    }
}
