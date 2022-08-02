using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _currentTilemap;
    [SerializeField] private TileBase[] _tiles;
    [SerializeField] private int _blockTileCount = 2;
    [ShowInInspector] private const int LEVEL_LENGTH = 11;
    [SerializeField, MinMaxSlider(1, LEVEL_LENGTH, true)] private Vector2Int _forkPossiblePosition = new Vector2Int(3, 7);
    [SerializeField, MinMaxSlider(2, 4, true)] private Vector2Int _forkPossibleLength = new Vector2Int(2, 4);
    [SerializeField, MinMaxSlider(2, 4, true)] private Vector2Int _biomLength = new Vector2Int(1, 4);
    private List<Block> _blocks = new List<Block>();
    [ShowInInspector] private Dictionary<Vector3Int, Sprite> _coordsSpriteDict = new Dictionary<Vector3Int, Sprite>();
    private int _currentBlockNumber;
    private TileBase _lastTile;
    private int _tileSize;
    
    
    /*public Texture2D atlas;    // Just to see on editor nothing to add from editor
    public Material testMaterial;
    public SpriteRenderer testSpriteRenderer;
    int textureWidthCounter = 0;
    int width,height;*/

    private void Start()
    {
        _tileSize = Mathf.RoundToInt(_currentTilemap.cellSize.y);
    }

    [GUIColor(0,1,0)]
    [Button("Generate level")]
    private void GenerateLevel()
    {
        ClearTilemap();
        GenerateBlocks();
        //GenerateCliffs();
    }
    
    private void ClearTilemap()
    {
        _currentTilemap.ClearAllTiles();
        _blocks.Clear();
    }

    private void GenerateBlocks()
    {
        var forkPosition = Random.Range(_forkPossiblePosition.x - 1, _forkPossiblePosition.y - 1);
        var forkLength = Random.Range(_forkPossibleLength.x - 1, _forkPossibleLength.y - 1);

        for (int i = 0; i < LEVEL_LENGTH; i++)
        {
            if (i < forkPosition || i > (forkPosition + forkLength))
            {
                _lastTile = _tiles[Random.Range(0, _tiles.Length - 1)];
                var block = new Block();
                _blocks.Add(block);
                block.SetBlock(new Vector3Int(0, i * _tileSize * _blockTileCount, 0), _currentTilemap, _lastTile,
                    _blockTileCount);
            }
            else
            {
                var leftTile = _tiles[Random.Range(0, _tiles.Length - 1)];
                var leftBlock = new Block();
                _blocks.Add(leftBlock);
                leftBlock.SetBlock(new Vector3Int(-_tileSize, i * _tileSize * _blockTileCount, 0), _currentTilemap,
                    leftTile, _blockTileCount);

                var rightTile = _tiles[Random.Range(0, _tiles.Length - 1)];
                var rightBlock = new Block();
                _blocks.Add(rightBlock);
                rightBlock.SetBlock(new Vector3Int(_tileSize, i * _tileSize * _blockTileCount, 0), _currentTilemap,
                    rightTile, _blockTileCount);
            }
        }
    }

    /*private void GenerateCliffs()
    {
        var tilePositions = _currentTilemap.cellBounds.allPositionsWithin;
        foreach (var position in tilePositions)
        {
            var sprite = _currentTilemap.GetSprite(position);
            if(sprite != null) _coordsSpriteDict[position] = _currentTilemap.GetSprite(position);
        }
        
        Sprite[] textures = _coordsSpriteDict.Values.ToArray();

        foreach (var t in textures)
        {
            width += t.texture.width;

            if (t.texture.height > height)
                height = t.texture.height;
        }
        
        atlas = new Texture2D(width,height, TextureFormat.RGBA32,false);
    
        for (int i = 0; i < textures.Length; i++)
        {
            int y = 0;

            while (y < atlas.height) {
                int x = 0;

                while (x < textures[i].texture.width ) {
                    if (y < textures[i].texture.height) 
                        atlas.SetPixel(x + textureWidthCounter, y, textures[i].texture.GetPixel(x, y));  // Fill your texture
                    else atlas.SetPixel(x + textureWidthCounter, y,new Color(0f,0f,0f,0f));  // Add transparency
                    x++;
                }
                y++;
            }
            atlas.Apply();
            textureWidthCounter +=  textures[i].texture.width;
        }
    
        // For normal renderers
        if (testMaterial != null)
            testMaterial.mainTexture = atlas;

        // for sprite renderer just make  a sprite from texture
        var s = Sprite.Create(atlas, new Rect(0f, 0f, atlas.width, atlas.height), new Vector2(0.5f, 0.5f));
        testSpriteRenderer.sprite = s;

        // add your polygon collider
        testSpriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
    }*/
}
