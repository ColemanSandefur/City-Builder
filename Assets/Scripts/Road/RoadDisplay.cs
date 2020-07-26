﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadDisplay : MonoBehaviour
{
    public RoadTileManager roadTileManager;
    public SpriteMask spriteMask;
    public SpriteRenderer spriteRenderer;
    public Sprite roadMaterial;
    public int currentMaskIndex;
    public Sprite[] m_Sprites;
    public Vector3Int location;
    public RoadMesh roadMesh;

    //Pathfinding
    public int gCost;
    public int hCost;
    public int fCost;
    public RoadDisplay cameFromNode;

    void Start() {
        spriteRenderer.sprite = roadMaterial;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }


    /*
        Tile Update Functions
    */
    public void Refresh(Tilemap tilemap, Vector3Int location) {
        spriteMask.sprite = GetCurrentSprite(location, tilemap);
        spriteRenderer.sprite = roadMaterial;
    }

    public bool HasRoadTile(Tilemap grid, Vector3Int position) {

        if (roadTileManager.roads.ContainsKey(position)) {
            return true;
        }
        // Transform parent = grid.transform;
        // var childCount = parent.childCount;
        // for (var i = 0; i < childCount; i++)
        // {
        //     var child = parent.GetChild(i);
        //     if (position == grid.WorldToCell(child.position) && child.GetComponent<RoadDisplay>() != null)
        //     {
        //         return true;
        //     }
        // }
        return false;
    }

    private Sprite GetCurrentSprite(Vector3Int location, Tilemap tilemap) {
        int mask = HasRoadTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0; // Top;
        mask += HasRoadTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0; // Right
        mask += HasRoadTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0; // Bottom
        mask += HasRoadTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0; // Left
        int index = GetIndex((byte)mask);
        currentMaskIndex = index;
        if (index >= 0 && index < m_Sprites.Length) {
            return m_Sprites[index];
        }
        else{
            return m_Sprites[0];
        }
    }

    // The following determines which sprite to use based on the number of adjacent RoadTiles
    private int GetIndex(byte mask)
    {
        switch (mask)
        {
            case 0: return 11; //none
            case 3: return 4;//(1, 1)
            case 6: return 5;//(1, -1)
            case 9: return 3;//(-1, 1)
            case 12: return 2; //(-1, -1)
            case 1: return 1;// (0, 1)
            case 2: return 0;// (1, 0)
            case 4: return 1;// (0, -1)
            case 5: return 1;// (0, -1) (0, 1)
            case 10: return 0;// (-1, 0) (1,0)
            case 8: return 0; //(-1,0)
            case 7: return 9;//(0, 1) (1, 0) (0, -1)
            case 11: return 8;//(0,1) (1,0) (-1,0)
            case 13: return 7;// (0,1) (0,-1) (-1,0)
            case 14: return 6; //(1,0) (0,-1) (-1,0)
            case 15: return 10; //(0,1) (1,0) (0,-1) (-1,0)
        }
        return -1;
    }
}
