using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    WorldMesh meshGenerator;
    WorldCell[] cells;
    List<Room> rooms;

    private void Awake()
    {
        rooms = new List<Room>();
        cells = new WorldCell[Metrics.worldSize.x * Metrics.worldSize.y];
        meshGenerator = GetComponentInChildren<WorldMesh>();
        if (meshGenerator == null)
        {
            Debug.LogError("Component WorldMesh not found in + " + gameObject.name);
        }
    }

    private void Start()
    {
        GenerateWorld();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegenerateWorld();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("World generated");
        }
    }

    void RegenerateWorld()
    {
        CleanCells();
        GenerateWorld();
        GenerateAllRooms();
        GenerateAllCorridors();

        meshGenerator.GenerateMesh(cells);
    }

    void GenerateWorld()
    {
        for(int y = 0; y < Metrics.worldSize.y; y++)
        {
            for(int x = 0; x < Metrics.worldSize.x; x++)
            {
                GameObject newCell = Instantiate(cellPrefab,transform);
                WorldCell cell = newCell.GetComponent<WorldCell>();
                cell.Instantiate(x, y, Metrics.cellSize);
                cells[x + y * Metrics.worldSize.x] = cell;
            }
        }
    }

    void CleanCells()
    {
        for(int aux = cells.Length - 1; aux >= 0; aux--)
        {
            Destroy(cells[aux].gameObject);
        }
    }

    void GenerateAllRooms()
    {
        rooms.Clear();
        int amountOfRooms = 0;
        while(amountOfRooms < Metrics.amountOfRooms)
        {
            if (GenerateRoom())
            {
                amountOfRooms++;
            }
        }
    }

    bool GenerateRoom()
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

    void GenerateAllCorridors()
    {
        rooms.Sort();

        for(int roomCorridor = 0; roomCorridor < rooms.Count - 1; roomCorridor++)
        {
            GenerateCorridor(rooms[roomCorridor].GetCenter(), rooms[roomCorridor + 1].GetCenter());
        }
    }

    void GenerateCorridor(Vector2Int start, Vector2Int end)
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

    Vector2Int HorizontalCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int currentPos = start;
        while(currentPos.x != end.x)
        {
            if(currentPos.x < end.x)
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


    Vector2Int VertialCorridor(Vector2Int start, Vector2Int end)
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

    WorldCell GetCellFromCoordinates(Vector2Int coords)
    {
        return cells[coords.x + coords.y * Metrics.worldSize.x];
    }
}
