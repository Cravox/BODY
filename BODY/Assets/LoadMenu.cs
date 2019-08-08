using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadMenu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(Load());
        }
    }

    private IEnumerator Load() {
        SoundController.Play(this.gameObject, SoundController.Voice.HAUKE);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.HAUKE).length);
        GameManager.instance.fadeAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
}