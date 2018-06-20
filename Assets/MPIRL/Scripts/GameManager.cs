using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

[System.Serializable]
public struct PerformerType {
  public AudioClip sample;
  public string generatorName;
}

public class GameManager : MonoBehaviour {
  public GameObject ballPrefab;

  public GameObject planePrefab;

  public Sequencer sequencer;

  public PerformerType[] performerTypes;

  private List<DetectedPlane> newPlanes;

  private GameObject ballRoot;
  private GameObject planeRoot;

  private bool started = false;

  void Awake() {
    newPlanes = new List<DetectedPlane>();

    ballRoot = new GameObject("Balls");
    planeRoot = new GameObject("Planes");

    Screen.sleepTimeout = SleepTimeout.NeverSleep;
  }
  
  void OnEnable() {
    SongStructure.Initialize();
    HarmonicProgression.Initialize(sequencer.numBars);

    sequencer.Play(AudioSettings.dspTime + 2.5);  
  }
  
  void OnDisable() {
    sequencer.Stop();  
  }

  void Update() {
    // Exit the app when the 'back' button is pressed.
    if (Input.GetKey(KeyCode.Escape)) {
      Application.Quit();
      return;
    }

    if (!sequencer.IsPlaying) {
      return;
    }

    // Check that motion tracking is tracking.
    if (Session.Status != SessionStatus.Tracking) {
        return;
    }
    // Iterate over planes found in this frame and instantiate corresponding planes.
    Session.GetTrackables<DetectedPlane>(newPlanes, TrackableQueryFilter.New);
    for (int i = 0; i < newPlanes.Count; ++i) {
      InitializeNewPlane(newPlanes[i]);
    }
	}

  public void ThrowBall(Vector3 initalVelocity) {
    var ball = GameObject.Instantiate(ballPrefab, ballRoot.transform);
    ball.transform.localPosition = Camera.main.transform.position;
    ball.GetComponent<Rigidbody>().AddForce(initalVelocity, ForceMode.VelocityChange);
  }

  private void InitializeNewPlane(DetectedPlane detectedPlane) {
    var plane = GameObject.Instantiate(planePrefab, planeRoot.transform);
    var planeController = plane.GetComponent<PlaneController>();

    planeController.Initialize(detectedPlane);

    var performerType = performerTypes[Random.Range(0, performerTypes.Length)];
    planeController.instrument.SetSample(performerType.sample);
    planeController.generator = GeneratorFactory.CreateGenerator(planeController.instrument, 
                                                                 performerType.generatorName);


    // TEST //
    if (!performerType.sample.name.StartsWith("drum")) {
      planeController.instrument.noteOffset = Random.Range(-8, 8);
    }
    if (performerType.generatorName == "OneBarNote") {
      (planeController.generator as OneBarNote).beatOffset = 0.5 * Random.Range(0, 8);
    }
    if (performerType.generatorName == "SimpleBassline") {
      (planeController.generator as SimpleBassline).beatOffset = Random.Range(0, 4);
    }
  }
}
