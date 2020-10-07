using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private Button resetButton;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(OozeEscape.Instance.ResetGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
