using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Level Spawner SO", menuName="Game State/Level Spawner")]
public class LevelSpawnScriptableObject : ScriptableObject
{
    public enum levelDifficulty
    {
        easy,
        medium,
        hard
    };

    public List<GameObject> easyChunks = new List<GameObject>();    // hold reference to easy level pieces
    public List<GameObject> mediumChunks = new List<GameObject>();  // hold reference to medium level pieces
    public List<GameObject> hardChunks = new List<GameObject>();    // hold reference to hard level piecs

    /// <summary>
    /// Get random level chunk prefab from lists by difficulty
    /// </summary>
    /// <param name="difficulty">desired difficulty of the level</param>
    /// <returns></returns>
    private GameObject GetRandomLevel(levelDifficulty difficulty)
    {
        switch (difficulty)
        {
            case levelDifficulty.easy:
                return easyChunks[Random.Range(0, easyChunks.Count)];
            case levelDifficulty.medium:
                return mediumChunks[Random.Range(0, easyChunks.Count)];
            case levelDifficulty.hard:
                return hardChunks[Random.Range(0, easyChunks.Count)];
            default:
                Debug.LogError("Unexpected difficulty parameter in GetRandomLevel");
                return null;
        }
    }

    /// <summary>
    /// spawn a new level with the origin at the specific end point
    /// </summary>
    /// <param name="difficulty">desired difficulty of the level</param>
    /// <param name="endpoint">endpoint of last level</param>
    /// <returns></returns>
    public GameObject CreateLevel(levelDifficulty difficulty, Transform endpoint, GameObject levelAnchor)
    {
        GameObject prefabToSpawn = GetRandomLevel(difficulty);
        
        GameObject newLevel = Instantiate(prefabToSpawn, endpoint.position, Quaternion.identity, levelAnchor.transform);
        return newLevel;
    }
}
