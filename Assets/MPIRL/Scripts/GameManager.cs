using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

public class GameManager : MonoBehaviour {
  public Camera mainCamera;

  public GameObject ballPrefab;

  public GameObject planePrefab;

  public Sequencer sequencer;

  public PerformerSelector performerSelector;

  private List<DetectedPlane> newPlanes;

  private GameObject ballRoot;
  private GameObject planeRoot;


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
    ball.transform.localPosition = mainCamera.transform.position + 0.1f * mainCamera.transform.forward;
    ball.GetComponent<Rigidbody>().AddForce(initalVelocity, ForceMode.VelocityChange);
  }

  private void InitializeNewPlane(DetectedPlane detectedPlane) {
    var anchor = detectedPlane.CreateAnchor(detectedPlane.CenterPose);
    anchor.transform.SetParent(planeRoot.transform);
    var plane = GameObject.Instantiate(planePrefab, anchor.transform);
    var planeController = plane.GetComponent<PlaneController>();

    planeController.Initialize(detectedPlane);
    
    var performerType = performerSelector.GetNextPerformerType();
    planeController.instrument.SetSample(performerType.sample);
    planeController.generator = GeneratorFactory.CreateGenerator(planeController.instrument, 
                                                                 performerType.generatorName);


    // TEST //
    if (performerType.generatorName == "Drums") {
      var generator = planeController.generator as Drums;
      if (performerType.sample.name.Contains("kick")) {
        generator.drumType = DrumType.KICK;
      } else if (performerType.sample.name.Contains("snare")) {
        generator.drumType = DrumType.SNARE;
      } else if (performerType.sample.name.Contains("clap")) {
        generator.drumType = DrumType.CLAP;
      } else if (performerType.sample.name.Contains("closed")) {
        generator.drumType = DrumType.HH_CLOSED;
      } else if (performerType.sample.name.Contains("open")) {
        generator.drumType = DrumType.HH_OPEN;
      }
    } else if (performerType.generatorName != "SimpleBassline") {
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
