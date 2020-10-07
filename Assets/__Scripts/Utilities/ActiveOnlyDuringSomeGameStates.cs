using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnlyDuringSomeGameStates : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private bool mainMenu;
    [SerializeField]
    private bool pause;
    [SerializeField]
    private bool play;
    [SerializeField]
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        StateUpdate(OozeEscape.Instance.CurrState);
        OozeEscape.Instance.OnChangeState += StateUpdate;
    }

    private void StateUpdate(OozeEscape.GameState state)
    {
        switch (state)
        {
            case OozeEscape.GameState.MainMenu:
                gameObject.SetActive(mainMenu);
                break;
            case OozeEscape.GameState.Pause:
                gameObject.SetActive(pause);
                break;
            case OozeEscape.GameState.Play:
                gameObject.SetActive(play);
                break;
            case OozeEscape.GameState.GameOver:
                gameObject.SetActive(gameOver);
                break;
        }
    }
}
