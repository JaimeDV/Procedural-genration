using System.Collections.Generic;
using UnityEngine;

public class S_GeneratePhysicalWorld : MonoBehaviour
{
    private Node[,] starGrid;

    [SerializeField]
    private GameObject wall;

    [SerializeField]
    private GameObject tree;

    [SerializeField]
    private GameObject ground;

    [SerializeField]
    private GameObject pickUp;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject player;

    private bool playerSpawned;
    private List<GameObject> foes;

    /// <summary>
    /// using the A*grid generates the physical world
    /// </summary>

    private void Awake()
    {
        foes = new List<GameObject>();
        MovingFoe[] movingFoe = GameObject.FindObjectsOfType<MovingFoe>();
        foreach (var foe in movingFoe)
        {
            foes.Add(foe.gameObject.transform.GetChild(0).gameObject);
            Debug.Log(foe.gameObject.transform.GetChild(0).gameObject);
        }
    }

    private void GeneratePhysicalWordl()
    {
        playerSpawned = false;
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
        PopulateWorld();
    }

    private void PopulateWorld() //fills the world with pickups players and enemies
    {
        foreach (var node in starGrid)
        {
            if (node != null)
            {
                var random = Random.Range(80, 300);
                var randomFill = Random.Range(0, 100);

                if (random <= randomFill && node.isNotWall)
                {
                    Instantiate(pickUp, node.position + new Vector3(0, 4.4f, 0), gameObject.transform.rotation);
                }

                else
                {
                    random = Random.Range(70, 300);
                    randomFill = Random.Range(0, 100);
                    if (random <= randomFill && node.isNotWall)
                    {
                        foreach (GameObject foe in foes)
                        {
                            if (foe != null && foe.activeSelf == false)
                            {
                                foe.SetActive(true);
                                foe.transform.position = node.position + new Vector3(0, 6, 0);

                                break;
                            }
                        }
                        //Instantiate(enemy, node.position + new Vector3(0, 6, 0), gameObject.transform.rotation);
                    }
                }

                 random = Random.Range(0, 300);
                randomFill = Random.Range(0, 100);

                if (random <= randomFill && node.isNotWall)
                {
                    Instantiate(tree, node.position + new Vector3(0, 14, 0), gameObject.transform.rotation);
                }
            }
        }
        while (playerSpawned == false) //there is only one player
        {
            foreach (var node in starGrid)
            {
                if (node != null)
                {
                    var random = Random.Range(98, 300);
                    var randomFill = Random.Range(0, 100);

                    if (random <= randomFill && node.isNotWall)
                    {
                        Instantiate(player, node.position + new Vector3(0, 5, 0), gameObject.transform.rotation);
                        playerSpawned = true;
                        break;
                    }
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