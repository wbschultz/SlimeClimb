using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSpawn : MonoBehaviour
{
    public Transform lastLevelEndpoint;

    [Header("Set in Inspector")]
    [SerializeField]
    private Transform firstSpawnPoint;
    [SerializeField]
    private LevelSpawnScriptableObject levelSpawnSO;
    [SerializeField]
    private int maxLevels = 6;

    private Queue<GameObject> levelQueue = new Queue<GameObject>();
    private GameObject _levelAnchor;
    private GameObject LevelAnchor
    {
        // get current level anchor or create new one
        get
        {
            if (_levelAnchor == null)
            {
                _levelAnchor = new GameObject("Level Anchor");
                return _levelAnchor;
            }
            else
            {
                return _levelAnchor;
            }
        }
        set
        {
            _levelAnchor = value;
        }
    }

    #region Public Methods
    /// <summary>
    /// Spawn level at last level's end point
    /// </summary>
    /// <param name="difficulty">desired difficulty of the level</param>
    /// <returns></returns>
    public void SpawnLevel(LevelSpawnScriptableObject.levelDifficulty difficulty)
    {
        GameObject nextLevel;

        if (lastLevelEndpoint == null)
            lastLevelEndpoint = firstSpawnPoint;

        // spawn levels
        for (int i = 0; i < 3; i++)
        {
            nextLevel = levelSpawnSO.CreateLevel(difficulty, lastLevelEndpoint, LevelAnchor);
            lastLevelEndpoint = nextLevel.transform.Find("Level End");

            // hack for destroying levels as we go
            levelQueue.Enqueue(nextLevel);
            if (levelQueue.Count > maxLevels)
            {
                Destroy(levelQueue.Dequeue());
            }
        }
    }

    public void DestroyLevels()
    {
        Destroy(LevelAnchor);
    }
    #endregion

    void Awake()
    {
        if (firstSpawnPoint == null)
        {
            Debug.LogError("Missing spawn point for first generated level");
        }
        lastLevelEndpoint = firstSpawnPoint;
    }




}
