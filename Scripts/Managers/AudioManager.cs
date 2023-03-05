using UnityEngine.Audio;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.Internal;

public class AudioManager : MonoBehaviour
{
    public SoundList[] soundList; // do not set this to static !! importing sounds in the unity editor will cease to function

    public static AudioManager instance { get; set; }

    string currentBGM; // only for BGM, not for sound effects

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DebugStats.AddLog("More than one Audio Manager in scene");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (SoundList sound in soundList)
        {
            sound.source = gameObject.AddComponent<AudioSource>(); // create all sounds and assign attributes

            sound.source.clip = sound.clip;

            sound.source.loop = sound.loopable;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    private void Start()
    {
        PlaySound("LcdDem OST - End", true);
    }

    private void Update()
    {
        //SoundList sound = Array.Find(soundList, sound => sound.fadingOut);

        foreach (SoundList sound in soundList) // seems inefficient ?
        {
            if (sound.fadingOut)
            {
                sound.source.volume -= 1f * Time.deltaTime;
            }
            if (sound.source.volume <= 0f)
            {
                sound.source.Stop();
                sound.fadingOut = false;
            }
        }
    }

    public void PlaySound(string name, bool isBGM)
    {
        if (isBGM) currentBGM = name;
        DebugStats.AddLog("Playing sound " + name);
        SoundList sound = Array.Find(soundList, sound => sound.name == name);
        try
        {
            sound.source.Play();
        }
        catch {
            ErrorFindingSound(name);
        }
    }

    public void FadeOutBGM()
    {
        DebugStats.AddLog("Fading out BGM...");
        FadeOutSound(currentBGM);
    }

    public void FadeOutSound(string name)
    {
        SoundList sound = Array.Find(soundList, sound => sound.name == name);
        try
        {
            sound.fadingOut = true;
        }
        catch
        {
            ErrorFindingSound(name);
        }
    }

    public void StopSound(string name)
    {
        SoundList sound = Array.Find(soundList, sound => sound.name == name);
        try
        {
            sound.source.Stop();
        }
        catch
        {
            ErrorFindingSound(name);
        }
    }

    private void ErrorFindingSound(string name)
    {
        DebugStats.AddLog("Couldn't find sound: " + name);
    }
}

[Serializable] public class SoundList
{
    public AudioClip clip; // could make data / list driven instead of the unity inspector stuff

    public string name;

    public bool loopable = false;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    public bool fadingOut = false;

    [HideInInspector] public AudioSource source;
}