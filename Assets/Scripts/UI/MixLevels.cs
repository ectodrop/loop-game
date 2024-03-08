using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixLevels : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    void Start()
    {
        // Load the saved volume levels, use a default value if not found
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f); // Default to 0.75 if not set
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f); // Default to 0.75 if not set

        SetMusicLevel(musicVolume);
        SetSFXLevel(sfxVolume);

        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    public void SetMusicLevel(float musicLvl)
    {
        float dB = LinearToDecibel(musicLvl);
        // Debug.Log($"Setting music level to: {musicLvl}");
        Debug.Log($"Setting music level to: {musicLvl}");
        masterMixer.SetFloat("MusicVol", dB);
        PlayerPrefs.SetFloat("MusicVolume", musicLvl);
    }

    public void SetSFXLevel(float sfxLvl)
    {
        float dB = LinearToDecibel(sfxLvl);
        // Debug.Log($"Setting music level to: {sfxLvl}");
        masterMixer.SetFloat("SFXVol", dB);
        PlayerPrefs.SetFloat("SFXVolume", sfxLvl);
    }

    // Converts a linear volume scale (0 to 1) to decibels (-80 to 0)
    private float LinearToDecibel(float linear)
    {
        if (linear <= 0) return -80f;
        return 20f * Mathf.Log10(linear);
    }

    // Convert back from dB to linear (e.g., for loading saved settings), if needed
    private float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
}

