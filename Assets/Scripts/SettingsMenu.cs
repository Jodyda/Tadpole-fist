using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;
    public void SetVolumeSFX(float volume)
    {
        sfxMixer.SetFloat("SFXvolume", volume);
        Debug.Log(volume);
    }

    public void SetVolumeMusic(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
    }
 
}
