using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_sc : MonoBehaviour
{
    public Slider BackVolume;
    public AudioSource audio;
    public GameObject Pannel;

    private float backVol = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Pannel.SetActive(false);
        backVol = BackVolume.value;
        PlayerPrefs.SetFloat("backvol", backVol);
    }

    // Update is called once per frame
    void Update()
    {
        SoundSlider();
    }

    public void SoundSlider()
    {
        audio.volume = BackVolume.value;

        backVol = BackVolume.value;
        PlayerPrefs.SetFloat("backvol", backVol);
    }

    public void SettingdOn()
    {
        Pannel.SetActive(true);
    }
    public void SettingdOff()
    {
        Pannel.SetActive(false);
    }
}
