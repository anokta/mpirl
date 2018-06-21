using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour {
  public PostProcessProfile virtualObjectsProfile;
  
  public float idleAberration = 0.0f;
  public float reactSpeed = 2.5f;

  private ChromaticAberration aberration;
  private float targetAbberation = 0.0f;
  
  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  void Start() {
    virtualObjectsProfile.TryGetSettings<ChromaticAberration>(out aberration);
  }

  void Update () {
    var currentAberration = aberration.intensity.value;
    if (Mathf.Abs(currentAberration - idleAberration) > 0.01f) {
      aberration.intensity.value = Mathf.Lerp(currentAberration, idleAberration, reactSpeed * Time.deltaTime);
    }
	}

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    targetAbberation = beat == 0 ? (bar == 0 ? 0.5f : 0.35f) : 0.25f;
    Invoke("Pulse", (float)(dspTime - AudioSettings.dspTime));
  }

  private void Pulse() {
    aberration.intensity.value = targetAbberation;
  }
}
