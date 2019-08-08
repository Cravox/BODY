using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : SerializedMonoBehaviour {
    private static SoundController Instance;
    
    [Required]
    public AudioClip[] Clips;

    [SerializeField]
    [Required]
    private AudioClip[] VoiceClips;

    public enum Sounds {
        CHAR_WALK01,
        CHAR_WALK02,
        CHAR_WALK03,
        CHAR_JUMP,
        CHAR_DOUBLEJUMP,
        CHAR_HOVER,
        CHAR_PICKUP,
        CHAR_THROW,
        CHAR_PUSH,
        BOX_IMPACT_SMALL,
        BOX_IMPACT_BIG,
        PUSHBOX_IMPACT,
        DOOR_OPENCLOSE,
        BUTTON_CLICK
    }

    public enum Voice {
        INTRO_1,
        INTRO_2,
        INTRO_3,
        CUBE_PICK_UP,
        CONSOLE_01,
        CONSOLE_02,
        JUMP_TUT01,
        JUMP_TUT02,
        BARRIER_TUT01,
        BARRIER_TUT02,
        BUTTON_PRESSED,
        BARRIER_TUT03,
        BARRIER_TUT04,
        PLATFORM_01,
        PLATFORM_02,
        BIG_CUBE_01,
        BIG_CUBE_02,
        BIG_CUBE_03,
        TUT_END01,
        TUT_END02,
        CUBE_RIGHT,
        HAUKE,
    }
    
    public static void Play(GameObject source, Sounds sound, int priority = 128, float volume = 1f) {
        PlaySound(source, Instance.Clips[(int) sound], priority, volume);
    }

    public static void Play(GameObject source, Voice voice, int priority = 128, float volume = 1f) {
        PlaySound(source, Instance.VoiceClips[(int) voice], priority, volume);
    }

    private static void PlaySound(GameObject source, AudioClip clip, int priority = 128, float volume = 1f) {
        var audioSource = source.AddComponent<AudioSource>();
        audioSource.priority = priority;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, clip.length);
    }

    public static AudioClip GetClip(Sounds s) {
        return Instance.Clips[(int) s];
    }

    public static AudioClip GetClip(Voice v) {
        return Instance.VoiceClips[(int)v];
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning(nameof(SoundController) + " is a singleton class and may only be initialized once");
            Application.Quit();
        }

        Instance = this;

        if (Enum.GetNames(typeof(Sounds)).Length != Clips.Length) {
            Debug.LogWarning("Enum item count of " + nameof(Sounds) + " in " + nameof(SoundController) + " has to be equal to the length of " + nameof(Clips), this);
            Application.Quit();
        }

        if (Clips.Any(c => c == null)) {
            Debug.LogWarning(nameof(Clips) + " in " + nameof(SoundController) + " may not contain null values", this);
            Application.Quit();
        }

        if (Enum.GetNames(typeof(Voice)).Length != VoiceClips.Length) {
            Debug.LogWarning("Enum item count of " + nameof(Voice) + " in " + nameof(SoundController) + " has to be equal to the length of " + nameof(VoiceClips), this);
            Application.Quit();
        }

        if (VoiceClips.Any(c => c == null)) {
            Debug.LogWarning(nameof(VoiceClips) + " in " + nameof(SoundController) + " may not contain null values", this);
            Application.Quit();
        }
    }
}