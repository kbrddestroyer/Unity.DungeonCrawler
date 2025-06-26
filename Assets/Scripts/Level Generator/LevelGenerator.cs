using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapDimensions;
    [SerializeField] private Vector2Int roomSizeMin;
    [SerializeField] private Vector2Int roomSizeMax;
    [SerializeField, Range(2, 100)] private int corridorWidth;
    [SerializeField, Range(100, 10000)] private uint iterationMax;
    [SerializeField] private Tilemap wallsMap;
    [SerializeField] private Tilemap baseTileMap;
    [SerializeField, Range(1, 100)] private int roomCount;
    [SerializeField] private RuleTile wallTile;
    [SerializeField] private RuleTile baseTile;
    [SerializeField] private GameObject player;
    
    [Serializable]
    private struct Room
    {
        [FormerlySerializedAs("Position")] public Vector3Int position;
        [FormerlySerializedAs("Bounds")] public Vector4 bounds;
    }

    [SerializeField] private List<Room> rooms = new();
    
    private static Vector4 ConvertRoomToWorldPosition(Room room) => new(
        room.position.x + room.bounds.x, room.position.y + room.bounds.y,
        room.position.x + room.bounds.z, room.position.y + room.bounds.w
    );

    private static bool Overlaps(Room l, Room r)
    {
        var lBounds = ConvertRoomToWorldPosition(l);
        var rBounds = ConvertRoomToWorldPosition(r);

        // Проверка пересечения прямоугольников
        return !(lBounds.z < rBounds.x || lBounds.x > rBounds.z || 
                 lBounds.w < rBounds.y || lBounds.y > rBounds.w);
    }
    
    private Vector4 GenerateRoom() => new(
        -Random.Range(roomSizeMin.x / 2, roomSizeMax.x / 2), -Random.Range(roomSizeMin.y / 2, roomSizeMax.y / 2), Random.Range(roomSizeMin.x / 2, roomSizeMax.x / 2),Random.Range(roomSizeMin.y / 2, roomSizeMax.y / 2)
    );
    
    private Vector3Int GenerateRoomPosition() => new(Random.Range(-mapDimensions.x / 2, mapDimensions.x / 2), Random.Range(-mapDimensions.y / 2, mapDimensions.y / 2), 0);

    private void GeneratePlayerStartRoom()
    {
        var room = new Room
        {
            position = wallsMap.WorldToCell(player.transform.position),
            bounds = GenerateRoom()
        };
        
        rooms.Add(room);
    }
    
    private void Generate()
    {
        uint counter = 0;
        
        while (rooms.Count < roomCount)
        {
            var room = new Room
            {
                position = GenerateRoomPosition(),
                bounds = GenerateRoom()
            };
            counter++;

            if (counter > iterationMax)
                break;
            
            if (rooms.Any(other => Overlaps(room, other)))
                continue;
            
            rooms.Add(room);
        }
    }
    
    private void GenerateCorridors()
    {
        var sortedRooms = rooms.OrderBy(r => Vector3.Distance(r.position, rooms[0].position)).ToList();

        for (var i = 0; i < sortedRooms.Count - 1; i++)
        {
            var nearestRoom = sortedRooms[i + 1];
            GenerateCorridorBetweenRooms(nearestRoom, sortedRooms[i]);
        }
    }

    private void GenerateCorridorBetweenRooms(Room room1, Room room2)
    {
        var start = room1.position;
        var end = room2.position;
        
        var horizontalFirst = Random.value > 0.5f;

        if (horizontalFirst)
        {
            CreateHorizontalCorridor(start.x, end.x, start.y);
            CreateVerticalCorridor(start.y, end.y, end.x);
        }
        else
        {
            CreateVerticalCorridor(start.y, end.y, start.x);
            CreateHorizontalCorridor(start.x, end.x, end.y);
        }
    }
    
    private void CreateHorizontalCorridor(int startX, int endX, int y)
    {
        var step = startX < endX ? 1 : -1;
        var widthOffset = corridorWidth / 2;

        for (var x = startX; x != endX + step; x += step)
        {
            for (var w = -widthOffset; w <= widthOffset; w++)
            {
                wallsMap.SetTile(new Vector3Int(x, y + w, 0), baseTile);
            }
        }
    }

    private void CreateVerticalCorridor(int startY, int endY, int x)
    {
        var step = startY < endY ? 1 : -1;
        var widthOffset = corridorWidth / 2;

        for (var y = startY; y != endY + step; y += step)
        {
            for (var w = -widthOffset; w <= widthOffset; w++)
            {
                wallsMap.SetTile(new Vector3Int(x + w, y, 0), baseTile);
            }
        }
    }

#if UNITY_EDITOR
    private void EditorDrawRoom(Room room)
    {
        for (var x = room.position.x + room.bounds.x; x <= room.position.x + room.bounds.z; x++)
        {
            for (var y = room.position.y + room.bounds.y; y <= room.position.y + room.bounds.w; y++)
            {
                wallsMap.SetTile(new Vector3Int((int) x, (int) y, 0), baseTile);
            }
        }
    }

    private void BuildWalls()
    {
        
    }

    private void EditorDraw()
    {
        foreach (var room in rooms)
            EditorDrawRoom(room);
    }

    public void GenerateNew()
    {
        Debug.Log("Drawing test rooms!");
        rooms.Clear();
        
        wallsMap.ClearAllTiles();
        
        GeneratePlayerStartRoom();
        Generate();
        GenerateCorridors();
        EditorDraw();
    }

    private void OnValidate()
    {
        if (!player)
            player = FindAnyObjectByType<Player>().gameObject;
    }
#endif
}
