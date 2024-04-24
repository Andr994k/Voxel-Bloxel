using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour
{
    public int mapSizeInChunks = 10;
    public int chunkSize = 16, chunkHeight = 100;
    public int chunkDrawingRange = 8;

    public GameObject chunkPrefab;

    public TerrainGenerator terrainGenerator;
    public Vector2Int mapSeedOffset;

    //public Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    //public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>();

    public UnityEvent onWorldCreated, onNewChunksGenerated;

    public Character character;
    public WorldData worldData { get; private set; }

    private void Awake()
    {
        StartCoroutine(LookForPlayer());

        worldData = new WorldData
        {
            chunkHeight = this.chunkHeight,
            chunkSize = this.chunkSize,
            chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>(),
            chunkDictionary = new Dictionary<Vector3Int, ChunkRenderer>()
        };
    }

    public void GenerateWorld()
    {
        GenerateWorld(Vector3Int.zero);
    }

    private void GenerateWorld(Vector3Int position)
    {

        WorldGenerationData worldGenerationData = GetPositionsThatPlayerSees(position);

        foreach (Vector3Int pos in worldGenerationData.chunkPositionsToRemove)
        {
            WorldDataHelper.RemoveChunk(this, pos);
        }
        

        foreach (Vector3Int pos in worldGenerationData.chunkDataPositionsToRemove)
        {
            WorldDataHelper.RemoveChunkData(this, pos);
        }

        foreach (var pos in worldGenerationData.chunkDataPositionsToCreate)
        {
            ChunkData data = new ChunkData(chunkSize, chunkHeight, this, pos);
            ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
            worldData.chunkDataDictionary.Add(pos, newData);
        }

        foreach (var pos in worldGenerationData.chunkPositionsToCreate)
        {
            ChunkData data = worldData.chunkDataDictionary[pos];
            MeshData meshData = Chunk.GetChunkMeshData(data);
            GameObject chunkObject = Instantiate(chunkPrefab, data.worldPosition, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            worldData.chunkDictionary.Add(data.worldPosition, chunkRenderer);
            chunkRenderer.InitializeChunk(data);
            chunkRenderer.UpdateChunk(meshData);

        }

        //foreach (ChunkData data in worldData.chunkDataDictionary.Values)
        //{
        //    MeshData meshData = Chunk.GetChunkMeshData(data);
        //    GameObject chunkObject = Instantiate(chunkPrefab, data.worldPosition, Quaternion.identity);
        //    ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
        //    worldData.chunkDictionary.Add(data.worldPosition, chunkRenderer);
        //    chunkRenderer.InitializeChunk(data);
        //    chunkRenderer.UpdateChunk(meshData);
        //}
        onWorldCreated?.Invoke();
    }



    internal BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromBlockCoords(this, x, y, z);
        ChunkData containerChunk = null;

        worldData.chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return BlockType.Nothing;
        Vector3Int blockInChunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, new Vector3Int(x, y, z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInChunkCoordinates);
    }

    private WorldGenerationData GetPositionsThatPlayerSees(Vector3Int playerPosition)
    {
        List<Vector3Int> allChunkPositionsNeeded = WorldDataHelper.GetChunkPositionsAroundPlayer(this, playerPosition);
        List<Vector3Int> allChunkDataPositionsNeeded = WorldDataHelper.GetDataPositionsAroundPlayer(this, playerPosition);

        List<Vector3Int> chunkPositionsToCreate = WorldDataHelper.SelectPositonsToCreate(worldData, allChunkPositionsNeeded, playerPosition);
        List<Vector3Int> chunkDataPositionsToCreate = WorldDataHelper.SelectDataPositonsToCreate(worldData, allChunkDataPositionsNeeded, playerPosition);

        List<Vector3Int> chunkPositionsToRemove = WorldDataHelper.GetUnneededChunks(worldData, allChunkPositionsNeeded);
        List<Vector3Int> chunkDataPositionsToRemove = WorldDataHelper.GetUnneededData(worldData, allChunkDataPositionsNeeded);

        WorldGenerationData data = new WorldGenerationData
        {
            chunkPositionsToCreate = chunkPositionsToCreate,
            chunkDataPositionsToCreate = chunkDataPositionsToCreate,

            chunkPositionsToRemove = chunkPositionsToRemove,
            chunkDataPositionsToRemove = chunkDataPositionsToRemove,
            chunkPositionsToUpdate = new List<Vector3Int>()
        };
        return data;
    }

    internal void LoadAdditionalChunksRequest(GameObject player)
    {
        Debug.Log("Load more chunks");
        GenerateWorld(Vector3Int.RoundToInt(player.transform.position));
        onNewChunksGenerated?.Invoke();
    }

    internal void RemoveChunk(ChunkRenderer chunk)
    {
        chunk.gameObject.SetActive(false);
    }

    internal bool SetBlock(RaycastHit hit, BlockType blockType)
    {

        ChunkRenderer chunk = hit.collider.GetComponent<ChunkRenderer>();
        if (chunk == null)
            return false;

        Vector3Int pos = GetBlockPos(hit);
        Debug.Log(character.isInEditorMode);
        if (blockType == BlockType.Air || character.isInEditorMode == true)
        {
            WorldDataHelper.SetBlock(chunk.chunkData.worldReference, pos, blockType);

        }
        else
        {
            if (hit.point[0] == pos[0] + 0.5)
            {
                //x face logic
                Vector3Int newpos = new Vector3Int(pos.x + 1, pos.y, pos.z);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
            if (hit.point[1] == pos[1] + 0.5)
            {
                //y
                Vector3Int newpos = new Vector3Int(pos.x, pos.y + 1, pos.z);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
            if (hit.point[2] == pos[2] + 0.5)
            {
                //z
                Vector3Int newpos = new Vector3Int(pos.x, pos.y, pos.z + 1);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
            if (hit.point[0] == pos[0] - 0.5)
            {
                //-x
                Vector3Int newpos = new Vector3Int(pos.x - 1, pos.y, pos.z);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
            if (hit.point[1] == pos[1] - 0.5)
            {
                //-y
                Vector3Int newpos = new Vector3Int(pos.x, pos.y - 1, pos.z);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
            if (hit.point[2] == pos[2] - 0.5)
            {
                //-z
                Vector3Int newpos = new Vector3Int(pos.x, pos.y, pos.z - 1);
                WorldDataHelper.SetBlock(chunk.chunkData.worldReference, newpos, blockType);
            }
        }
        chunk.ModifiedByThePlayer = true;
        
        if (Chunk.IsOnEdge(chunk.chunkData, pos))
        {
            List<ChunkData> neighbourDataList = Chunk.GetEdgeNeighbourChunk(chunk.chunkData, pos);
            foreach (ChunkData neighbourData in neighbourDataList)
            {
                ChunkRenderer chunkToUpdate = WorldDataHelper.GetChunk(neighbourData.worldReference, neighbourData.worldPosition);
                if (chunkToUpdate != null)
                    chunkToUpdate.UpdateChunk();
            }

        }
        
        chunk.UpdateChunk();
        return true;
    }

    public Vector3Int GetBlockPos(RaycastHit hit)
    {
        Vector3 pos = new Vector3(
             GetBlockPositionIn(hit.point.x, hit.normal.x),
             GetBlockPositionIn(hit.point.y, hit.normal.y),
             GetBlockPositionIn(hit.point.z, hit.normal.z)
             );

        return Vector3Int.RoundToInt(pos);
    }

    private float GetBlockPositionIn(float pos, float normalpos)
    {
        if (Mathf.Abs(pos % 1) == 0.5f)
        {
            pos -= (normalpos / 2);
        }


        return (float)pos;
    }

    public struct WorldGenerationData
    {
        public List<Vector3Int> chunkPositionsToCreate;
        public List<Vector3Int> chunkDataPositionsToCreate;
        public List<Vector3Int> chunkPositionsToRemove;
        public List<Vector3Int> chunkDataPositionsToRemove;
        public List<Vector3Int> chunkPositionsToUpdate;
    }

    public struct WorldData
    {
        public Dictionary<Vector3Int, ChunkData> chunkDataDictionary;
        public Dictionary<Vector3Int, ChunkRenderer> chunkDictionary;
        public int chunkSize;
        public int chunkHeight;
    }
    IEnumerator LookForPlayer()
    {
        yield return new WaitForSeconds(3f);
        character = GameObject.Find("Player(Clone)").GetComponent<Character>();
    }
}
