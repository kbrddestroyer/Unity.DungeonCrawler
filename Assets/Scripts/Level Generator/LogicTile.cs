using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Logic Tile for Generator", menuName = "Tiles/Rule Tile")]
public class LogicTile : RuleTile
{
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        return base.RuleMatch(neighbor, other);
    }
}
