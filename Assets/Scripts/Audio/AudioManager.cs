using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
    [SerializeField] private float fadeTime = 1f;
    private Coroutine coroutine;

    private string song;
    public static AudioManager instance;
    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        // Format sounds
        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;

            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.ignoreListenerPause = sound.ignorePause;

            sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
        }

        // Persist between scenes
        DontDestroyOnLoad(this);
    }

    private IEnumerator FadeInAudio(Sound sound)
    {
        var startVolume = 0;
        var endVolume = sound.volume;
        float timer = 0;

        // Play source
        sound.audioSource.Play();

        while (timer < fadeTime)
        {
            // Lerp volume towards max
            sound.audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / fadeTime);

            // Decrement time
            timer += Time.deltaTime;
            yield return null;
        }

        // Set to desired volume
        sound.audioSource.volume = sound.volume;
    }

    private IEnumerator FadeOutAudio(Sound sound)
    {
        var startVolume = sound.volume;
        var endVolume = 0;
        float timer = 0;

        while (timer < fadeTime)
        {
            // Lerp volume towards max
            sound.audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / fadeTime);

            // Decrement time
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop playing
        sound.audioSource.Stop();

        // Reset volume
        sound.audioSource.volume = sound.volume;

    }

    public void PlayMusic(string name)
    {
        // Don't replay same song
        if (song == name) return;

        Sound sound = sounds.Find(sound => sound.name == name);
        if (sound != null)
        {
            this.song = name;
            if (coroutine != null) StopCoroutine(coroutine);

            coroutine = StartCoroutine(FadeInAudio(sound));
        }
        else { print("Sound with name " + name + " not found."); }
    }

    public void StopMusic(string name)
    {
        Sound sound = sounds.Find(sound => sound.name == name);
        if (sound != null)
        {
            // Remove song
            song = "";

            if (coroutine != null) StopCoroutine(coroutine);

            coroutine = StartCoroutine(FadeOutAudio(sound));
        }
        else { print("Sound with name " + name + " not found."); }
    }

    public void PlaySound(string name)
    {
        // Find sound
        Sound sound = sounds.Find(sound => sound.name == name);

        // Make sure sound exists
        if (sound != null)
        {
            // Set volume
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;

            // Play sound
            sound.audioSource.Play();
        }
        else { print("Sound with name " + name + " not found."); }
    }

    public void StopSound(string name)
    {
        // Find sound
        Sound sound = sounds.Find(sound => sound.name == name);

        // Make sure sound exists
        if (sound != null)
        {
            // Stop sound
            sound.audioSource.Stop();
        }
        else { print("Sound with name " + name + " not found."); }
    }
}
