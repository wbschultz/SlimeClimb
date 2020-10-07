using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LevelSpawn))]
public class OozeEscape : SingletonBase<OozeEscape>
{

    // notify subscribers fn state changes
    public event Action<GameState> OnChangeState;

    //[Header("Set in Inspector")]
    //[SerializeField]
    //private List<GameObject> resetPosOnGameOver = new List<GameObject>();

    private GameState _currState = GameState.MainMenu;
    public GameState CurrState
    {
        get
        {
            return _currState;
        }
        private set
        {
            if (_currState != value)
            {
                _currState = value;
                OnChangeState?.Invoke(_currState);
            }
        }
    }

    private LevelSpawn levelSpawner;
    private GameObject player;
    private float initPlayerY;
    private Camera mainCam;
    private Vector3 playerInitPos;
    //private Dictionary<GameObject, Transform> initPositions;

    // describe the state of the game, useful for AODSGS
    public enum GameState
    {
        Play,
        Pause,
        MainMenu,
        GameOver
    };

    public override void Awake()
    {
        base.Awake();
        levelSpawner = GetComponent<LevelSpawn>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInitPos = player.transform.position;

        // play music
        GetComponent<AudioManager>().Play("game");

        // get camera 
        mainCam = Camera.main;

        // bind ondeath function
        player.GetComponent<Player>().OnDeath += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if (CurrState == GameState.Play)
        {
            // check player distance to last level end point
            if (levelSpawner.lastLevelEndpoint.position.y - player.transform.position.y < mainCam.orthographicSize * 2)
            {
                // TEMP HACK difficulty is constant
                levelSpawner.SpawnLevel(LevelSpawnScriptableObject.levelDifficulty.easy);
            }
        }
    }

    private void GameOver()
    {
        CurrState = GameState.GameOver;
    }

    public void StartGame()
    {
        /*
        // get initial positions for dictionary
        if (initPositions == null)
        {
            initPositions = new Dictionary<GameObject, Transform>();
            foreach (GameObject obj in resetPosOnGameOver)
            {
                initPositions.Add(obj, obj.transform);
            }
        }
        */

        CurrState = GameState.Play;

        // start tracking position
        initPlayerY = player.transform.position.y;

        // spawn levels
        levelSpawner.SpawnLevel(LevelSpawnScriptableObject.levelDifficulty.easy);

        // start music
        
        // player start animation/particles
        
    }

    public void ResetGame()
    {
        /*
        foreach(KeyValuePair<GameObject, Transform> kvp in initPositions)
        {
            kvp.Key.transform.position = kvp.Value.position;
            kvp.Key.transform.rotation = kvp.Value.rotation;
        }
        */
        mainCam.transform.position = new Vector3(0, 0, -10);
        player.GetComponent<UnityJellySprite>().SetPosition(playerInitPos, true);
        levelSpawner.DestroyLevels();
        CurrState = GameState.MainMenu;
    }
}
