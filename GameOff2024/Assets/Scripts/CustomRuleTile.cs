using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor>
{
    [SerializeField]
    private bool alwaysConnect;

    [SerializeField]
    private TileBase[] tilesToConnect;

    [SerializeField]
    private bool checkSelf = true;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        try
        {
            switch (neighbor)
            {
                case Neighbor.This: return Check_This(tile);
                case Neighbor.NotThis: return Check_NotThis(tile);
                case Neighbor.Any: return Check_Any(tile);
                case Neighbor.Specified: return Check_Specified(tile);
                case Neighbor.Nothing: return Check_Nothing(tile);
            }
            return base.RuleMatch(neighbor, tile);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RuleMatch: " + ex);
            return false;
        }
    }

    bool Check_This(TileBase tile)
    {
        try
        {
            if (!alwaysConnect) return tile == this;
            else return tilesToConnect.Contains(tile) || tile == this;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Check_This: " + ex);
            return false;
        }
    }

    bool Check_NotThis(TileBase tile)
    {
        try
        {
            if (!alwaysConnect) return tile != this;
            else return !tilesToConnect.Contains(tile) && tile != this;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Check_NotThis: " + ex);
            return false;
        }
    }
    bool Check_Any(TileBase tile)
    {
        try
        {
            if (checkSelf)
                return tile != null;
            else
                return tile != null && tile != this;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Check_Any: " + ex);
            return false;
        }
    }

    bool Check_Specified(TileBase tile)
    {
        try
        {
            return tilesToConnect.Contains(tile);
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Check_Specified: " + ex);
            return false;
        }

        
    }

    bool Check_Nothing(TileBase tile)
    {
        try
        {
            return tile == null;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Check_Nothing: " + ex);
            return false;
        }        
    }
}
