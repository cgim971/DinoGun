using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MapCategory
{
    PATH = 0,
    BLOCK = 1,
    SPAWNPOINT = 2,

    NONE = 99
}

public class MapManager
{
    public static MapManager Instance;

    private Tilemap _mainMap;
    private Tilemap _collisionMap;
    private Tilemap _spawnpointMap;

    public MapManager(Transform mapObject)
    {
        _mainMap = mapObject.Find("Ground").GetComponent<Tilemap>();
        _collisionMap = mapObject.Find("Collision").GetComponent<Tilemap>();
        _spawnpointMap = mapObject.Find("Spawnpoint").GetComponent<Tilemap>();
    }

    public Vector3Int GetTilePosition(Vector3 worldPos) => _mainMap.WorldToCell(worldPos);
    public MapCategory GetTileCategory(Vector3Int tilePos)
    {
        TileBase tile = null;

        tile = _collisionMap.GetTile(tilePos);
        if (tile != null) return MapCategory.BLOCK;

        tile = _spawnpointMap.GetTile(tilePos);
        if (tile != null) return MapCategory.SPAWNPOINT;

        tile = _mainMap.GetTile(tilePos);
        if (tile != null) return MapCategory.PATH;
        else return MapCategory.NONE;
    }
}
