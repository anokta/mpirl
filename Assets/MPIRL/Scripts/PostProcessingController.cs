using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour {
  public PostProcessProfile mainProfile;
  public PostProcessProfile virtualObjectsProfile;

  public float idleMainGrain = 0.5f;
  //public float idleGrainSize = 2.0f;
  
  public float idleAberration = 0.0f;

  public float reactSpeed = 2.5f;

  private Grain mainGrain;
  private float targetMainGrain = 0.0f;

  //private float currentGrainSize = 0.0f;

  private ChromaticAberration aberration;
  private float targetAbberation = 0.0f;

  private Vignette vignette;
  private float currentVignette = 0.0f;
  
  void OnEnable() {
    Sequencer.OnNextBeat += OnNextBeat;
    
    //currentGrainSize = idleGrainSize;
  }
  
  void OnDisable() {
    Sequencer.OnNextBeat -= OnNextBeat;
  }

  void Start() {
    mainProfile.TryGetSettings<Grain>(out mainGrain);
    mainProfile.TryGetSettings<Vignette>(out vignette);
    virtualObjectsProfile.TryGetSettings<ChromaticAberration>(out aberration);
  }

  void Update () {
    float currentMainGrain = mainGrain.intensity.value;
    if (Mathf.Abs(currentMainGrain - idleMainGrain) > 0.01f) {
      mainGrain.intensity.value = Mathf.Lerp(currentMainGrain, idleMainGrain, reactSpeed * Time.deltaTime);
    }
    float currentAberration = aberration.intensity.value;
    if (Mathf.Abs(currentAberration - idleAberration) > 0.01f) {
      aberration.intensity.value = Mathf.Lerp(currentAberration, idleAberration, reactSpeed * Time.deltaTime);
    }

    float targetVignette = Mathf.Min(0.85f, 0.05f * GameManager.numBalls);
    if (Mathf.Abs(targetVignette - currentVignette) > 0.01f) {
      currentVignette = Mathf.Lerp(currentVignette, targetVignette, reactSpeed * Time.deltaTime);
      vignette.intensity.value = currentVignette;
    }

    //float targetGrainSize = Mathf.Min(20.0f, 0.5f * GameManager.numPlanes);
    //if (Mathf.Abs(targetGrainSize - currentGrainSize) > 0.01f) {
    //  currentGrainSize = Mathf.Lerp(currentGrainSize, targetGrainSize, reactSpeed * Time.deltaTime);
    //  mainGrain.size.value = currentGrainSize;
    //}
	}

  private void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    targetMainGrain = beat == 0 ? (bar == 0 ? 1.0f : 0.8f) : 0.7f;
    targetAbberation = beat == 0 ? (bar == 0 ? 0.5f : 0.35f) : 0.25f;
    Invoke("Pulse", (float)(dspTime - AudioSettings.dspTime));
  }

  private void Pulse() {
    mainGrain.intensity.value = targetMainGrain;
    aberration.intensity.value = targetAbberation;
  }
}
