using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapDimensions;
    [SerializeField] private Vector2Int roomSizeMin;
    [SerializeField] private Vector2Int roomSizeMax;
    [SerializeField, Range(2, 100)] private int corridorWidth;
    [SerializeField, Range(100, 10000)] private uint iterationMax;
    [SerializeField] private Tilemap tilemap;
    [SerializeField, Range(1, 100)] private int roomCount;
    [SerializeField] private RuleTile ruleTile;
    [SerializeField] private GameObject player;

    private struct Vector4Int
    {
        // ReSharper disable InconsistentNaming
        public int x, y, z, w;
        // ReSharper restore InconsistentNaming

        public Vector4Int(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
    
    [Serializable]
    private struct Room
    {
        public Vector3Int position;
        public Vector4Int Bounds;
    }

    [SerializeField] private List<Room> rooms = new();
    
    private static Vector4Int ConvertRoomToWorldPosition(Room room) => new(
        room.position.x + room.Bounds.x, room.position.y + room.Bounds.y,
        room.position.x + room.Bounds.z, room.position.y + room.Bounds.w
    );

    private static bool Overlaps(Room l, Room r)
    {
        var lBounds = ConvertRoomToWorldPosition(l);
        var rBounds = ConvertRoomToWorldPosition(r);

        return !(lBounds.z < rBounds.x || lBounds.x > rBounds.z || 
                 lBounds.w < rBounds.y || lBounds.y > rBounds.w);
    }
    
    private Vector4Int GenerateRoom() => new(
        -Random.Range(roomSizeMin.x / 2, roomSizeMax.x / 2), -Random.Range(roomSizeMin.y / 2, roomSizeMax.y / 2), Random.Range(roomSizeMin.x / 2, roomSizeMax.x / 2),Random.Range(roomSizeMin.y / 2, roomSizeMax.y / 2)
    );
    
    private Vector3Int GenerateRoomPosition() => new(Random.Range(-mapDimensions.x / 2, mapDimensions.x / 2), Random.Range(-mapDimensions.y / 2, mapDimensions.y / 2), 0);

    private void GeneratePlayerStartRoom()
    {
        var room = new Room
        {
            position = tilemap.WorldToCell(player.transform.position),
            Bounds = GenerateRoom()
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
                Bounds = GenerateRoom()
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
        
        var widthOffset = corridorWidth / 2;
        
        var stepX = start.x < end.x ? 1 : -1;
        var stepY = start.y < end.y ? 1 : -1;
        
        FillBox(new Vector4Int(
            start.x, 
            start.y - widthOffset, 
            end.x + widthOffset * stepX, 
            start.y + widthOffset)
        );
        FillBox(new Vector4Int(
            end.x - widthOffset, 
            start.y + widthOffset * stepY, 
            end.x + widthOffset, 
            end.y)
        );
    }

    private void FillBox(Vector4Int bounds)
    {
        var stepX = bounds.x < bounds.z ? 1 : -1;
        var stepY = bounds.y < bounds.w ? 1 : -1;
        
        for (var x = bounds.x; x != bounds.z + stepX; x += stepX)
        {
            for (var y = bounds.y; y != bounds.w + stepY; y += stepY)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), ruleTile);
            }
        }
    }

    private void DrawRoom(Room room)
    {
        var bounds = ConvertRoomToWorldPosition(room);
        FillBox(bounds);
    }

    private void DrawAll()
    {
        foreach (var room in rooms)
            DrawRoom(room);
    }

    public void GenerateNew()
    {
        rooms.Clear();
        tilemap.ClearAllTiles();
        
        GeneratePlayerStartRoom();
        Generate();
        GenerateCorridors();
        DrawAll();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!player)
            player = FindAnyObjectByType<Player>().gameObject;
    }
#endif
}
