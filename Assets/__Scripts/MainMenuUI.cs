using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private Button volume;
    [SerializeField]
    private Button play;
    [SerializeField]
    private GameObject volumeSlider;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(OozeEscape.Instance.StartGame);
        volume.onClick.AddListener(() => {
            volumeSlider.SetActive(!volumeSlider.activeInHierarchy);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
