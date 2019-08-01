using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class ScreenShakeManager : SerializedMonoBehaviour {
    public static ScreenShakeManager Instance;

    [SerializeField]
    [Required]
    private CinemachineFreeLook freeLook;

    private readonly CinemachineBasicMultiChannelPerlin[] noises = new CinemachineBasicMultiChannelPerlin[3];
    private float resetTime;

    private void Start() {
        if (Instance != null) {
            Debug.LogError("There may be only one screen shake manager");
        }

        Instance = this;
        for (var i = 0; i < 3; i++) {
            noises[i] = freeLook.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update() {
        if (resetTime < Time.time) {
            StopShake();
        }
    }

    public void Shake(float amplitude = 1f, float frequency = 2, float duration = 0.1f) {
        noises.ForEach(n => n.m_AmplitudeGain = amplitude);
        noises.ForEach(n => n.m_FrequencyGain = frequency);
        resetTime = Time.time + duration;
    }

    private void StopShake() {
        noises.ForEach(n => n.m_AmplitudeGain = 0f);
    }

    public IEnumerator AttachCamera(UnityAction action, Transform transform, float delay = 0f) {
        yield return new WaitForSeconds(delay);
        freeLook.Follow = transform;
        freeLook.LookAt = transform;
        yield return new WaitForEndOfFrame();
        action.Invoke();
    }

    public IEnumerator AttachCamera(GameObject oldGameObject, Transform transform, float delay = 0f) {
        var beginPos = oldGameObject.transform.position;
        var progress = 0f;
        do {
            oldGameObject.transform.position = Vector3.Lerp(beginPos, transform.position, progress);
            progress += Time.deltaTime / delay;
            yield return new WaitForEndOfFrame();
        } while (progress < 1f);
        
        freeLook.Follow = transform;
        freeLook.LookAt = transform;
        
        Destroy(oldGameObject);
    }
}
