using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private TextMeshProUGUI scoreTMP;
    [SerializeField]
    private TextMeshProUGUI hiScoreTMP;

    private GameObject player;
    private float initPlayerY;
    private float yDist;

    private const string HI_SCORE_PPKEY = "hiscore";


    // Start is called before the first frame update
    void OnEnable()
    {
        //TODO link this with the game state change events 
        player = GameObject.FindGameObjectWithTag("Player");
        initPlayerY = player.transform.position.y;

        yDist = Mathf.Max(player.transform.position.y - initPlayerY, yDist);

        // show high score
        hiScoreTMP.text = "HIGH: " + PlayerPrefs.GetInt(HI_SCORE_PPKEY).ToString("F0") + "m";
        // show first score
        scoreTMP.text = Mathf.Floor(yDist).ToString("F0") + "m";
    }

    private void OnDisable()
    {
        SetHighScore();
        yDist = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreTMP.text = Mathf.Floor(yDist).ToString("F0") + "m";
    }

    private void FixedUpdate()
    {
        if (OozeEscape.Instance.CurrState == OozeEscape.GameState.Play)
        {
            yDist = Mathf.Max(player.transform.position.y - initPlayerY, yDist);
        }
    }

    private void SetHighScore()
    {
        // get high score from playerPrefs
        int prevScore = PlayerPrefs.GetInt(HI_SCORE_PPKEY);
        int currScore = (int)Mathf.Floor(yDist);
        // check which is higher
        if (currScore > prevScore)
        {
            // new highscore
            PlayerPrefs.SetInt(HI_SCORE_PPKEY, currScore);
            hiScoreTMP.text = "HIGH: " + PlayerPrefs.GetInt(HI_SCORE_PPKEY).ToString("F0") + "m";
        }
    }
}
