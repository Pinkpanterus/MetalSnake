using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public sealed class Block
{
    private List<Vector3Int> _tilesCoords = new List<Vector3Int>();

    public void SetBlock(Vector3Int position, Tilemap tilemap, TileBase tile, int blockTileCount)
    {
        var tileSize = Mathf.RoundToInt(tilemap.cellSize.y);

        for (int i = 0; i < blockTileCount; i++)
        {
            for (int j = 0; j < blockTileCount; j++)
            {
                var pos = new Vector3Int(position.x + i * tileSize, position.y + j * tileSize, position.z);
                tilemap.SetTile(pos, tile);
                _tilesCoords.Add(pos);
            }
        }
    }
}
