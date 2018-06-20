﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

public class GameManager : MonoBehaviour {
  public GameObject ballPrefab;

  public GameObject planePrefab;

  public Sequencer sequencer;

  public AudioClip[] clips;

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

    sequencer.Play(AudioSettings.dspTime + 4.0f);  
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
    planeController.generator = 
      GeneratorFactory.CreateGenerator(planeController.instrument, "OneBarNote");
    

    // TEST //
    planeController.instrument.noteOffset = Random.Range(-8, 8);
    planeController.instrument.SetSample(clips[Random.Range(0, clips.Length)]);
    (planeController.generator as OneBarNote).beatOffset = 0.5 * Random.Range(0, 8);
  }
}
