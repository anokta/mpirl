using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneBarNote : Generator {
  public double beatOffset = 0.0;
  public int noteOffset = 0;

  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    double relativeBeat = beatOffset - beat;
    if (relativeBeat >= 0.0 && relativeBeat < 1.0) {
      double playTime = dspTime + relativeBeat * beatTime;
      instrument.PlayNote(playTime, HarmonicProgression.GetHarmonic(section, bar) + noteOffset);
    }
  }
}
