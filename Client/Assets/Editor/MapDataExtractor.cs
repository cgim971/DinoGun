using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class MapDataExtractor
{
    // % ctrl, # shift & alt
    [MenuItem("Tools/ExtractMap %&e")]
    private static void ExtractMapData()
    {
        GameObject tilemap = GameObject.Find("Tilemap");
        if (tilemap == null)
        {
            Debug.LogError("There is no tilemap in hierachy");
            return;
        }

        Tilemap tmCollision = tilemap.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap tmSpawnpoint = tilemap.transform.Find("Spawnpoint").GetComponent<Tilemap>();

        tmCollision.CompressBounds();

        using (StreamWriter writer = File.CreateText($"Assets/Resources/Map/{tilemap.name}.txt"))
        {
            BoundsInt mapBound = tmCollision.cellBounds;

            writer.WriteLine(mapBound.xMin);
            writer.WriteLine(mapBound.xMax);
            writer.WriteLine(mapBound.yMin);
            writer.WriteLine(mapBound.yMax);

            for (int y = mapBound.yMax - 1; y >= mapBound.yMin; y--)
            {
                for (int x = mapBound.xMin; x < mapBound.xMax; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    TileBase tile = tmCollision.GetTile(tilePos);

                    if (tile != null)
                    {
                        writer.Write("1");
                    }
                    else
                    {
                        TileBase spawnTile = tmSpawnpoint.GetTile(tilePos);
                        writer.Write(spawnTile == null ? "0" : "2");
                    }
                }
                writer.WriteLine("");
            }
        }
    }
}