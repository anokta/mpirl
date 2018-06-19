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
    if (Input.GetMouseButtonUp(0)) {
      Vector3 velocity = mainCamera.transform.forward;
      velocity.y += Random.Range(0.0f, 0.25f);
      gameManager.ThrowBall(3.0f * velocity);
      return;
    }
	}
  
}
