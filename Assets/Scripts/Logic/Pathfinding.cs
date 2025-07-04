using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Pathfinding : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("walking");
    [SerializeField] private Tilemap walls;
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Logical")] 
    [SerializeField, Range(0f, 10f)] private float reactionDistance;
    [SerializeField, Range(0f, 10f)] private float rerouteDistance;
    [SerializeField, Range(0f, 10f)] private float stopDistance;
    [SerializeField, Range(0f, 10f)] private float speed;

    [Header("Event system")] 
    [SerializeField] private UnityEvent onStartMove;
    [SerializeField] private UnityEvent onReachedDestination;
    
    private Stack<Vector3> _path;
    
    private class Waypoint {
        public Vector3Int Point;
        public readonly Waypoint Parent;
        public readonly float Weight;
        
        public Waypoint(Vector3Int point, Waypoint parent, ref Tilemap walls, ref Player player)
        {
            Point = point;
            Parent = parent;
            Weight = Vector2.Distance(walls.LocalToWorld(point), player.transform.position);
        }
    }

    private void Start()
    {
        _path = new Stack<Vector3>();
    }
    
    private class PriorityQueue<TOb> : Dictionary<float, TOb> { 
        public TOb Minimum()
        {
            if (Count == 0)
                return default;

            var minimum = Keys.ToList()[0];
            minimum = Keys.ToList().Prepend(minimum).Min();

            var obj = this[minimum];
            Remove(minimum);

            return obj;
        }
    }
    
    private Waypoint GetPathFromDestinationCoordinates(Vector3 destination)
    {
        var bounds = walls.cellBounds;

        var sourceTile = walls.WorldToCell(transform.position);
        var destinationTile = walls.WorldToCell(destination);

        var closed = new List<Vector3Int>();
        var open = new PriorityQueue<Waypoint>();

        var start = new Waypoint(sourceTile, null, ref walls, ref player);

        open[start.Weight] = start;
        
        while (open.Count > 0)
        {
            var p = open.Minimum();
            if (p.Point == destinationTile)
            {
                return p;
            }
            if (closed.Contains(p.Point))
            {
                continue;
            }

            for (var i = -1; i <= 1; i ++)
                for (var j = -1; j <= 1; j ++)
                {
                    if (i == 0 && j == 0) continue;

                    var x = p.Point.x + i;
                    var y = p.Point.y + j;

                    if (x < bounds.x || x > bounds.max.x || y < bounds.y || y > bounds.max.y)
                        continue;

                    var pos = new Vector3Int(x, y, p.Point.z);
                    var tile = walls.GetTile(pos);
                    if (tile)
                    {
                        TileData tileData = new();
                        tile.GetTileData(pos, walls, ref tileData);
                        if (
                            tileData.colliderType != Tile.ColliderType.None
                        ) continue;
                    }

                    var point = new Waypoint(pos, p, ref walls, ref player);
                    open[point.Weight] = point;
                }
            closed.Add(p.Point);
        }

        return null;
    }

    protected Stack<Vector3> GetPath(Vector3 destination)
    {
        var waypoints = new Stack<Vector3>();
        
        var arr = GetPathFromDestinationCoordinates(destination);
        
        if (arr == null) return waypoints;
        while (arr != null)
        {
            waypoints.Push(walls.CellToLocal(arr.Point) + walls.cellSize * 0.5f);
            arr = arr.Parent;
        }

        return waypoints;
    }

    private void Update()
    {
        var distance = Vector2.Distance(transform.position, player.transform.position);
        if (_path.Count > 0 && distance > reactionDistance)
        {
            _path.Clear();
            animator.SetBool(Walking, false);
        }
        else if (distance <= reactionDistance)
        {
            if (distance <= stopDistance)
            {
                if (_path.Count > 0)
                    _path.Clear();
                onReachedDestination.Invoke();
                animator.SetBool(Walking, false);
            }
            
            if (_path.Count == 0 && distance > stopDistance)
            {
                _path = GetPath(player.transform.position);

                if (_path == null)
                    return;
                
                if (_path.Count == 0) return;
                
                onStartMove.Invoke();
                animator.SetBool(Walking, true);
                if (_path.Peek().x < transform.position.x)
                    spriteRenderer.flipX = true;
            }

            if (_path.Count == 0) return;
            
            var waypoint = _path.Peek();

            transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

            if (!(Vector2.Distance(waypoint, transform.position) <= rerouteDistance)) return;
            
            _path.Pop();
            if (_path.Count > 0)
                spriteRenderer.flipX = _path.Peek().x < transform.position.x;
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!player)
            player = FindAnyObjectByType<Player>();

        if (!animator)
            animator = GetComponent<Animator>();
        
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (!walls)
            walls = GameObject.FindGameObjectWithTag("Walls").GetComponent<Tilemap>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (!player) return;
        if (!walls) return;
        
        var arr = GetPathFromDestinationCoordinates(player.transform.position);
        var waypoints = new List<Vector3>();

        while (arr != null)
        {
            waypoints.Add(walls.CellToLocal(arr.Point) + walls.cellSize * 0.5f);
            arr = arr.Parent;
        }

        var waypointsSpan = new ReadOnlySpan<Vector3>(waypoints.ToArray());
        Gizmos.DrawLineStrip(waypointsSpan, false);
        
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
        Gizmos.DrawWireSphere(transform.position, rerouteDistance);
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
#endif
}
