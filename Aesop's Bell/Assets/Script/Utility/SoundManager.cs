using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;      // For background music
    public AudioSource effectsSource;    // For sound effects

    [Header("Music Clips")]
    public AudioClip menuTitleMusic;
    public AudioClip screamJamMusic;

    [Header("Sound Effects")]
    public AudioClip deathSFX;
    public AudioClip keyPickupSFX;
    public AudioClip mouseSFX;
    public AudioClip walkingSFX;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float effectsVolume = 1f;

    private void Awake()
    {
        // Singleton pattern to ensure only one SoundManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolumeSettings();  // Set initial volume levels
    }

    // Play a specific music clip by reference
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip is null.");
        }
    }

    // Stop the background music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Play a specific sound effect by reference
    public void PlayEffect(AudioClip effectClip)
    {
        if (effectClip != null)
        {
            effectsSource.PlayOneShot(effectClip, effectsVolume);
        }
        else
        {
            Debug.LogWarning("Sound effect clip is null.");
        }
    }

    // Adjust music volume dynamically
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    // Adjust effects volume dynamically
    public void SetEffectsVolume(float volume)
    {
        effectsVolume = Mathf.Clamp01(volume);
        effectsSource.volume = effectsVolume;
    }

    // Apply the current volume settings
    private void ApplyVolumeSettings()
    {
        musicSource.volume = musicVolume;
        effectsSource.volume = effectsVolume;
    }
}
