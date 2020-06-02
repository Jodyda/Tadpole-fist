using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour {

    private SoundManager sound;
    public Button musicToggleButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;


    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.FindObjectOfType<SoundManager>();
        UpdateIconAndVolume();
    }

    public void PauseMusic()
    {
        sound.ToggleSound(); // update Player prefs
        UpdateIconAndVolume();
    }

    void UpdateIconAndVolume()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            AudioListener.volume = 1.0f;
            musicToggleButton.GetComponent<Image>().sprite = musicOnSprite;
        } else
        {
            AudioListener.volume = 0.0f;
            musicToggleButton.GetComponent<Image>().sprite = musicOffSprite;
        }
    }
}
