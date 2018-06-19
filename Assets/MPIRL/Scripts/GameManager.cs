using System.Collections;
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

  private List<DetectedPlane> newPlanes;

  private GameObject planeRoot;

  private void Awake() {
    newPlanes = new List<DetectedPlane>();
    planeRoot = new GameObject("Planes");
  }

  void Start() {
		
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
      var plane = GameObject.Instantiate(planePrefab, planeRoot.transform);
      plane.GetComponent<PlaneController>().Initialize(newPlanes[i]);
    }
	}
}
