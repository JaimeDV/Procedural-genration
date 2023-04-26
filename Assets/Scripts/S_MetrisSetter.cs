using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MetrisSetter : MonoBehaviour
{

    /// <summary>
    /// changes the values of Metrics on the editor
    /// </summary>
    [SerializeField]
    public  Vector2 cellSize = new Vector2(10f, 10f);
    [SerializeField]
    public  Vector2Int worldSize = new Vector2Int(50, 50);
    [SerializeField]
    public  Vector2Int minRoomsSize = new Vector2Int(5, 5);
    [SerializeField]
    public  Vector2Int maxRoomsSize = new Vector2Int(15, 15);
    [SerializeField]
    public  int amountOfRooms = 5;

    // BSP variables
    [SerializeField]
    public  int desiredPartitionAmount = 3;

    // Celullar automata
    [SerializeField]
    public  int neightborAmount = 8;

    // Drunken walk
    [SerializeField]
    public  int walks = 20;
    [SerializeField]
    public  int walkDistance = 50;

    private void Awake()
    {
        Metrics.cellSize = cellSize;
        Metrics.worldSize = worldSize;
        Metrics.minRoomsSize = minRoomsSize;
        Metrics.maxRoomsSize = maxRoomsSize;
        Metrics.amountOfRooms=amountOfRooms;
        Metrics.desiredPartitionAmount = desiredPartitionAmount;
        Metrics.neightborAmount=neightborAmount;
        Metrics.walks=walks;
        Metrics.walkDistance=walkDistance;
    }
    private void Update()
    {
        Metrics.cellSize = cellSize;
        Metrics.worldSize = worldSize;
        Metrics.minRoomsSize = minRoomsSize;
        Metrics.maxRoomsSize = maxRoomsSize;
        Metrics.amountOfRooms = amountOfRooms;
        Metrics.desiredPartitionAmount = desiredPartitionAmount;
        Metrics.neightborAmount = neightborAmount;
        Metrics.walks = walks;
        Metrics.walkDistance = walkDistance;
    }
}
