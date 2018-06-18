using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour {
  // Uses quarter notes for time signature, i.e., "numBeats/4".
  public double bpm = 120.0;
  public int numBeats = 4;
  public int numBars = 4;
  
  public int CurrentBeat { get; private set; }
  public int CurrentBar { get; private set; }
  public int CurrentSection { get; private set; }
  public bool IsPlaying { get; private set; }

  private double numSecondsPerBeat = 0;
  private double timeOffsetSeconds = 0;

  private double lastDspTime = 0.0;


  public void Play() {
    CurrentBeat = 0;
    CurrentBar = 0;
    CurrentSection = 0;
    timeOffsetSeconds = 0.0;
    lastDspTime = AudioSettings.dspTime;
    IsPlaying = true;
  }

  public void Stop() {
    IsPlaying = false;
    lastDspTime = 0.0f;
  }

  void Update() {
    //Debug.Log(CurrentSection + " / " + CurrentBar + " / " + CurrentBeat + " -- " + timeOffsetSeconds);
    //if (Input.GetKeyDown(KeyCode.Space)) {
    //  if (IsPlaying) {
    //    Stop();
    //  } else {
    //    Play();
    //  }
    //}

    if (!IsPlaying) {
      return;
    }

    double currentDspTime = AudioSettings.dspTime;
    timeOffsetSeconds += currentDspTime - lastDspTime;
    lastDspTime = currentDspTime;

    double newBeats = System.Math.Floor(timeOffsetSeconds / numSecondsPerBeat);
    CurrentBeat += (int) newBeats;
    timeOffsetSeconds -= newBeats * numSecondsPerBeat;

    if (numBeats == 0) {
      return;
    }
    CurrentBar += CurrentBeat / numBeats;
    CurrentBeat %= numBeats;

    if (numBars == 0) {
      return;
    }
    CurrentSection += CurrentBar / numBars;
    CurrentBar %= numBars;
  }

  void OnValidate() {
    CalculateNumSamplesPerBeat();
  }

  private void CalculateNumSamplesPerBeat() {
    numSecondsPerBeat = (bpm > 0.0) ? 60.0 / bpm : 0.0;
    if (numSecondsPerBeat > 0.0) {
      timeOffsetSeconds -= System.Math.Floor(timeOffsetSeconds / numSecondsPerBeat) * numSecondsPerBeat;
    }
  }
}
