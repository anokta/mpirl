using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour {
  public delegate void SequencerEvent(int section, int bar, int beat, double dspTime, double beatTime);
  public static event SequencerEvent OnNextBeat;

  // Uses quarter notes for time signature, i.e., "numBeats/4".
  public double bpm = 120.0;
  public int numBeats = 4;
  public int numBars = 4;

  private int currentBeat = 0;
  private int currentBar = 0;
  private int currentSection = 0;
  
  public bool IsPlaying { get; private set; }

  private double numSecondsPerBeat = 0.0;

  private double targetDspTime = 0.0;
  private double lookaheadSeconds = 0.0;
  private bool hasStarted = false;


  void Awake() {
    lookaheadSeconds = 2.0 * (double)Time.fixedUnscaledDeltaTime;
    CalculateNumSamplesPerBeat();
  }

  void Update() {
    if (!hasStarted || numSecondsPerBeat <= 0.0f) {
      return;
    }

    double currentDspTime = AudioSettings.dspTime;
    while (currentDspTime + lookaheadSeconds >= targetDspTime) {
      TriggerNextBeat();
      ++currentBeat;
      targetDspTime += numSecondsPerBeat;
      if (numBeats != 0) {
        currentBar += currentBeat / numBeats;
        currentBeat %= numBeats;
      }
      if (numBars != 0) {
        currentSection += currentBar / numBars;
        currentBar %= numBars;
      }
    }
  }

  public void Play(double startDspTime) {
    currentBeat = 0;
    currentBar = 0;
    currentSection = 0;
    targetDspTime = startDspTime;
    
    hasStarted = true;
  }

  public void Stop() {
    hasStarted = false;
    IsPlaying = false;
  }

  private void CalculateNumSamplesPerBeat() {
    double previousTarget = targetDspTime - numSecondsPerBeat;
    numSecondsPerBeat = (bpm > 0.0) ? 60.0 / bpm : 0.0;

    // This seems buggy, turning it off as it's not really used anyway.
    //if (numSecondsPerBeat > 0.0) {
    //  double currentDspTime = AudioSettings.dspTime;
    //  double timeOffset = currentDspTime - previousTarget;
    //  targetDspTime = currentDspTime + numSecondsPerBeat - (timeOffset - System.Math.Floor(timeOffset / numSecondsPerBeat) * numSecondsPerBeat);
    //}
  }

  private void TriggerNextBeat() {
    IsPlaying = true;
    if (OnNextBeat != null) {
      OnNextBeat(currentSection, currentBar, currentBeat, targetDspTime, numSecondsPerBeat);
    }
  }
}
