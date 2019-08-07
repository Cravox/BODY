using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour {
    [SerializeField]
    private SoundController.Voice voiceClip;

    [SerializeField]
    private bool barrier;

    [SerializeField]
    private bool carryBox;

    [SerializeField]
    private bool isEnergywall;

    [SerializeField]
    private bool platform;

    [SerializeField]
    private bool BBCSpeech;

    [SerializeField]
    private bool BBC;

    [SerializeField]
    private bool end;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (barrier) {
            SoundController.Play(Camera.main.gameObject, voiceClip);
            GetComponent<BoxCollider>().enabled = false;
        }

        if (other.CompareTag("Carry") && carryBox) {
            SoundController.Play(Camera.main.gameObject, voiceClip);
            GetComponent<BoxCollider>().enabled = false;
        }

        if (platform) {
            StartCoroutine(PlatformSpeech());
            GetComponent<BoxCollider>().enabled = false;
        }

        if (BBCSpeech) {
            StartCoroutine(CubeSpeech());
            GetComponent<BoxCollider>().enabled = false;
        }

        if (BBC) {
            if (other.CompareTag("Push")) {
                SoundController.Play(Camera.main.gameObject, voiceClip);
                GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (end) {
            StartCoroutine(TEST());
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator PlatformSpeech() {
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.PLATFORM_01);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.PLATFORM_01).length);
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.PLATFORM_02);
    }

    IEnumerator CubeSpeech() {
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.BIG_CUBE_01);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.BIG_CUBE_01).length);
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.BIG_CUBE_02);
    }

    IEnumerator TEST() {
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.TUT_END01);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.TUT_END01).length);
        SoundController.Play(Camera.main.gameObject, SoundController.Voice.TUT_END02);
    }
}
