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
        COMBAT_DRONE_DESTROYED,
        COMBAT_EVAC_ACTIVE,
        COMBAT_HEALTH_CRITICAL,
        COMBAT_MODULE_READY,
        COMBAT_OVERCHARGE_ACTIVE,
        COMBAT_REPAIR_ACTIVE,
        COMBAT_ROCKETS_READY,
        COMBAT_SHIELD_DOWN,
        COMBAT_SHIELD_RECHARGED,
        COMBAT_STEALTH_ACTIVE,
        COMBAT_WEAPON_OVERHEATED,
        GENERAL_ACTIVATE_GENERATOR,
        GENERAL_ACTIVATE_WORKSHOP,
        GENERAL_CLUSTER_DESTROYED,
        GENERAL_NCUS_OBTAINED,
        GENERAL_START_GAME,
        MISSION_ACTIVATE_TRANSMITTER,
        MISSION_CLUSTER_DEACTIVATED,
        MISSION_CLUSTER_SELF_DESTRUCT,
        MISSION_CORE_OBTAINED,
        MISSION_DEFEND_SHELTER,
        MISSION_ENEMIES_DEFEATED,
        MISSION_FIND_WATER,
        MISSION_MATERIALS_OBTAINED,
        MISSION_MISSION_2,
        MISSION_MISSION_3,
        MISSION_PREPARE_PLATFORM,
        MISSION_REACTIVATE_SHIELD,
        MISSION_SECURE_PATH,
        MISSION_SECURE_SHELTER,
        MISSION_SHELTER_UNDER_ATTACK,
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