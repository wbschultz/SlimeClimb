using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(setVolume);
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.value = AudioListener.volume;
    }

    public void setVolume(float vol)
    {
        AudioListener.volume = vol;
    }
}
