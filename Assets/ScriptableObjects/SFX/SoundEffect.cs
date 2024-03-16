using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SoundEffect", fileName = "NewSFX")]
public class SoundEffect : ScriptableObject
{
    public AudioClip[] audioClips;
    public AudioMixerGroup group;
    [Range(0, 1f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    private AudioSource _currentAudioSource;
    public AudioClip GetAudioClip()
    {
        if (audioClips.Length == 0) return null;
        if (audioClips.Length == 1) return audioClips[0];
        // pick a random audio clip
        int rand = Random.Range(0, audioClips.Length - 1);
        return audioClips[rand];
    }
    
    public void Play(AudioSource source = null)
    {
        if (audioClips.Length == 0) return;
        bool destroy = false;
        if (source == null)
        {
            var tmp = new GameObject("sfx", typeof(AudioSource));
            source = tmp.GetComponent<AudioSource>();
            source.playOnAwake = false;
            destroy = true;
        }
        source.Stop();

        source.clip = GetAudioClip();
        source.pitch = pitch;
        source.outputAudioMixerGroup = group;
        source.volume = volume;
        source.Play();
        if (destroy)
        {
            Destroy(source.gameObject, source.clip.length / source.pitch);
        }
    }
}
