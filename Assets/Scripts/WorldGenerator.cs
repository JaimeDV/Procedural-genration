using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    private WorldMesh meshGenerator;
    private WorldCell[] cells;
    private List<WorldCell> openCells;
    private List<Room> rooms;

    public static event System.Action regenerateGrid;
    public static event System.Action destroyWorld;
    public static event System.Action createWorld;
    private string pathfindingTag = "PathFinding"; //it works i don't know how , don't touch it

    private void Awake()
    {
        rooms = new List<Room>();
        openCells = new List<WorldCell>();
        cells = new WorldCell[Metrics.worldSize.x * Metrics.worldSize.y];
        meshGenerator = GetComponentInChildren<WorldMesh>();
        if (meshGenerator == null)
        {
            Debug.LogError("Component WorldMesh not found in + " + gameObject.name);
        }
    }

    private void Start()
    {
        GenerateWorldDrunkenWalk();
        SmoothCellular(2);
        //GenerateWorldDrunkenWalk();
        regenerateGrid();
        createWorld();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CleanPathfinding();
            ReGenerateWorldDrunkenWalk();
            SmoothCellular(2);
            CleanPathfinding();
            regenerateGrid();
            destroyWorld();
            createWorld();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("World generated");
        }
    }

    private void GenerateCellularAutomata()/// creates celullar automata based on https://bronsonzgeb.com/index.php/2022/01/30/procedural-generation-with-cellular-automata/ & https://www.youtube.com/watch?v=v7yyZZjF1z4&ab_channel=SebastianLague
    {
        GenerateWorld();
        generateCelullarPath();

        meshGenerator.GenerateMesh(cells);
    }

    private void CleanPathfinding()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(pathfindingTag);
        foreach (GameObject go in cubes)
        {
            Destroy(go);
        }
    }

    private void generateCelullarPath() //generates a celullar grid based on https://www.youtube.com/watch?v=v7yyZZjF1z4&t=656s&ab_channel=SebastianLague
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int y = 0; y < Metrics.worldSize.y; y++)
        {
            for (int x = 0; x < Metrics.worldSize.x; x++)
            {
                int index = x + y * Metrics.worldSize.x;
                GameObject newCell = Instantiate(cellPrefab, transform);
                WorldCell cell = newCell.GetComponent<WorldCell>();
                cell.InstantiateCell(x, y, Metrics.cellSize);
                cells[index] = cell;
                if (x == 0 || x == Metrics.worldSize.x - 1 || y == 0 || y == Metrics.worldSize.y - 1)
                {
                    cell.Close();
                }
                else
                {
                    var random = Random.Range(0, 100);
                    var randomFill = Random.Range(0, 100);
                    if (random <= randomFill)
                    {
                        cell.Open();
                    }
                }
                //int liveCellCount = cell +  cell.GetNeighbour(cell,index);
            }
        }
    }

    private void SmoothCellular(int iterations)//cellular smothing for all other algortihms i only test it on druken walk
    {
       
        WorldCell cell;
        int index;
        for (int z = 0; z < iterations; z++)
        {
            for (int y = 0; y < Metrics.worldSize.y; y++)
            {
                for (int x = 0; x < Metrics.worldSize.x; x++)
                {
                    index = x + y * Metrics.worldSize.x;
                    cell = cells[index];
                    int neighbourTiles=GetSurrodingWalls(y, x);
                    Debug.Log(neighbourTiles);
                    if (neighbourTiles>=4 && neighbourTiles<=8)
                    {
                        var random = Random.Range(0, 50);
                        var randomFill = Random.Range(0, 100);
                        if (random <= randomFill)
                        {
                            Debug.Log("Closing");
                            cell.Open();
                        }
                    }
                    else
                    {
                        var random = Random.Range(0, 100);
                        var randomFill = Random.Range(0, 100);
                        if (random <= randomFill)
                        {
                            cell.Close();
                        }
                    }
                }
            }
        }
    }

    private int GetSurrodingWalls(int gridY, int gridX)
    {
        int walls = 0;
        WorldCell[] cellsCopy;
        WorldCell cell;
        int index = gridX + gridY * Metrics.worldSize.x;

        cellsCopy = cells;
        cell= cellsCopy[index];
        for (int neigbourY = gridY - 1; neigbourY <= gridY + 1; neigbourY++)
        {
            {
                for (int neigbourX = gridX - 1; neigbourX <= gridX + 1; neigbourX++)
                {
                    index = neigbourX + neigbourY * Metrics.worldSize.x;
                    if (neigbourX >= 0 && neigbourY >= 0 && neigbourY<Metrics.worldSize.y && neigbourX< Metrics.worldSize.x)
                    {

                        if (neigbourX != gridX || neigbourY != gridY)
                        {
                            if (cells[index].isEmpty!)
                            {
                                walls++; //IS WALL
                            }

                          
                        }
                    }
                    
                    
                }
            }
        }
        return walls;
    }

    private void GenerateWorldDrunkenWalk()
    {
        GenerateWorld();

        // Open center cell
        Vector2Int coords = new Vector2Int(Metrics.worldSize.x / 2, Metrics.worldSize.y / 2);

        int drunkCounter = 0;
        WorldCell centerCell = GetCellFromCoordinates(coords);
        centerCell.Open();
        openCells.Add(centerCell);

        while (drunkCounter < Metrics.walks)
        {
            GenerateDrunkPath();
            drunkCounter++;
        }

        meshGenerator.GenerateMesh(cells);
    }

    private void ReGenerateWorldDrunkenWalk()
    {
        CleanCells();
        openCells.Clear();
        GenerateWorldDrunkenWalk();
    }

    private void GenerateWorldBSP()
    {
        GenerateWorld();

        RoomTreeNode partitionTree = ChopSpace();
        GenerateAllRoomsBSP(partitionTree);
        GenerateAllCorridors();

        meshGenerator.GenerateMesh(cells);
    }

    private void RegenerateWorldBSP()
    {
        CleanCells();
        rooms.Clear();
        GenerateWorldBSP();
    }

    private void RegenerateWorldCA()
    {
        CleanCells();
        rooms.Clear();
        GenerateWorldCA();
    }

    private void GenerateWorldCA()
    {
        GenerateWorld();
        OrderWorld();
        meshGenerator.GenerateMesh(cells);
    }

    private void OrderWorld()
    {
        foreach (WorldCell cell in cells)
        {
            int amount = cell.EmptyNeightbors();
            if (amount <= 0 || amount > 4)
            {
                cell.Close();
            }
            else
            {
                cell.Open();
            }
        }
    }

    private void GenerateDrunkPath()
    {
        // Get a random open cell
        WorldCell curentCell = openCells[Random.Range(0, openCells.Count)];
        int walkLenght = 0;

        while (walkLenght <= Metrics.walkDistance)
        {
            // Select a random neigtbor
            curentCell = curentCell.GetRandomNeightbor();
            if (curentCell == null) // Got to a border
            {
                return;
            }
            curentCell.Open(); // Open new cell
            openCells.Add(curentCell);
            walkLenght++;
        }
    }

    private void RegenerateWorldBasicRoomGeneration()
    {
        CleanCells();
        GenerateWorld();
        GenerateAllRooms();
        GenerateAllCorridors();

        meshGenerator.GenerateMesh(cells);
    }

    private void GenerateWorld()
    {
        for (int y = 0; y < Metrics.worldSize.y; y++)
        {
            for (int x = 0; x < Metrics.worldSize.x; x++)
            {
                int index = x + y * Metrics.worldSize.x;
                GameObject newCell = Instantiate(cellPrefab, transform);
                WorldCell cell = newCell.GetComponent<WorldCell>();
                cell.InstantiateCell(x, y, Metrics.cellSize);
                cells[index] = cell;

                // Has something on left (not 1st column)
                if (x != 0 || x % Metrics.worldSize.x != 0)
                {
                    cell.SetNeightbor(cells[index - 1], Neightbors.Left);
                }
                // Has somthing below (not 1st row)
                if (y != 0)
                {
                    cell.SetNeightbor(cells[index - Metrics.worldSize.x], Neightbors.Down);
                    // Has something below left (not 1st column)
                    if (x != 0 || x % Metrics.worldSize.x != 0)
                    {
                        cell.SetNeightbor(cells[index - Metrics.worldSize.x - 1], Neightbors.DownLeft);
                    }
                    // Has something below right (not last column)
                    if ((x + 1) % Metrics.worldSize.x != 0)
                    {
                        cell.SetNeightbor(cells[index - Metrics.worldSize.x + 1], Neightbors.DownRight);
                    }
                }
            }
        }
    }

    //un used
    //WorldCell[,] GenerateWorldGiver() //tries to give the grid to the A* algoritm
    //{
    //    WorldCell[,] logicGrid;
    //    logicGrid = new WorldCell[Metrics.worldSize.x, Metrics.worldSize.y];
    //    for (int y = 0; y < Metrics.worldSize.y; y++)
    //    {
    //        for (int x = 0; x < Metrics.worldSize.x; x++)
    //        {
    //            int index = x + y * Metrics.worldSize.x;
    //            GameObject newCell = Instantiate(cellPrefab, transform);
    //            WorldCell cell = newCell.GetComponent<WorldCell>();
    //            cell.InstantiateCell(x, y, Metrics.cellSize);

    //            cells[index] = cell;

    //            // Has something on left (not 1st column)
    //            if (x != 0 || x % Metrics.worldSize.x != 0)
    //            {
    //                cell.SetNeightbor(cells[index - 1], Neightbors.Left);
    //            }
    //            // Has somthing below (not 1st row)
    //            if (y != 0)
    //            {
    //                cell.SetNeightbor(cells[index - Metrics.worldSize.x], Neightbors.Down);
    //                // Has something below left (not 1st column)
    //                if (x != 0 || x % Metrics.worldSize.x != 0)
    //                {
    //                    cell.SetNeightbor(cells[index - Metrics.worldSize.x - 1], Neightbors.DownLeft);
    //                }
    //                // Has something below right (not last column)
    //                if ((x + 1) % Metrics.worldSize.x != 0)
    //                {
    //                    cell.SetNeightbor(cells[index - Metrics.worldSize.x + 1], Neightbors.DownRight);
    //                }
    //            }
    //            logicGrid[y, x] = cell;
    //        }
    //    }
    //    return logicGrid;
    //}

    private RoomTreeNode ChopSpace()
    {
        RoomTreeNode worldPartition = new RoomTreeNode(cells, Metrics.worldSize);
        RoomTreeNode[] leafPartitions;
        int partitionCounter = 0;

        while (partitionCounter < Metrics.desiredPartitionAmount)
        {
            leafPartitions = worldPartition.GetLeafs();
            foreach (RoomTreeNode node in leafPartitions)
            {
                node.Chop();
            }
            partitionCounter++;
        }

        return worldPartition;
    }

    private void GenerateAllRoomsBSP(RoomTreeNode tree)
    {
        RoomTreeNode[] leafPartitions = tree.GetLeafs();
        foreach (RoomTreeNode node in leafPartitions)
        {
            rooms.Add(node.GenerateBSPRoom());
        }
    }

    private void CleanCells()
    {
        for (int aux = cells.Length - 1; aux >= 0; aux--)
        {
            Destroy(cells[aux].gameObject);
        }
    }

    private void GenerateAllRooms()
    {
        rooms.Clear();
        int amountOfRooms = 0;
        while (amountOfRooms < Metrics.amountOfRooms)
        {
            if (GenerateRoom())
            {
                amountOfRooms++;
            }
        }
    }

    private bool GenerateRoom()
    {
        Room newRoom = new Room();
        newRoom.GenerateRandomRoom();
        if (newRoom.CalculateRoomCells(cells))
        {
            rooms.Add(newRoom);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GenerateAllCorridorsNonLineal()
    {
        for (int exp = 1; exp <= Metrics.desiredPartitionAmount; exp++)
        {
            for (int index = 0; index < rooms.Count;)
            {
                int jump = (int)Mathf.Pow(2, exp);
                GenerateCorridor(rooms[index].GetCenter(), rooms[index + jump - 1].GetCenter());
                index = index + jump;
            }
        }
    }

    private void GenerateAllCorridors()
    {
        rooms.Sort();

        for (int roomCorridor = 0; roomCorridor < rooms.Count - 1; roomCorridor++)
        {
            GenerateCorridor(rooms[roomCorridor].GetCenter(), rooms[roomCorridor + 1].GetCenter());
        }
    }

    private void GenerateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int halfCorridorCoords;
        if (Random.Range(0, 2) == 0) // Horizontal 1st
        {
            halfCorridorCoords = HorizontalCorridor(start, end);
            VertialCorridor(halfCorridorCoords, end);
        }
        else // Vertical 1st
        {
            halfCorridorCoords = VertialCorridor(start, end);
            HorizontalCorridor(halfCorridorCoords, end);
        }
    }

    private Vector2Int HorizontalCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int currentPos = start;
        while (currentPos.x != end.x)
        {
            if (currentPos.x < end.x)
            {
                currentPos.x++;
            }
            else
            {
                currentPos.x--;
            }

            WorldCell currentCell = GetCellFromCoordinates(currentPos);
            currentCell.Fill();
            currentCell.SetColor(Color.white);
        }
        return currentPos;
    }

    private Vector2Int VertialCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int currentPos = start;
        while (currentPos.y != end.y)
        {
            if (currentPos.y < end.y)
            {
                currentPos.y++;
            }
            else
            {
                currentPos.y--;
            }

            WorldCell currentCell = GetCellFromCoordinates(currentPos);
            currentCell.Fill();
            currentCell.SetColor(Color.white);
        }
        return currentPos;
    }

    private WorldCell GetCellFromCoordinates(Vector2Int coords)
    {
        return cells[coords.x + coords.y * Metrics.worldSize.x];
    }
}