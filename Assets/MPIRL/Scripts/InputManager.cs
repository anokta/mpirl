using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
  public GameManager gameManager;

  public Camera mainCamera;

	void Update () {
    //if (Input.touchCount == 0) {
    //  return;
    //}
    //Touch touch = Input.GetTouch(0);

    // TODO(anokta): This is temporary, replace via flick -> throw behaivor.

#if UNITY_EDITOR
    if (Input.GetMouseButtonUp(0)) {
#else
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
#endif  // UNITY_EDITOR
      Vector3 velocity = mainCamera.transform.forward;
      velocity.y += Random.Range(0.0f, 0.1f);
      gameManager.ThrowBall(5.0f * velocity);
      return;
    }
	}
  
}
