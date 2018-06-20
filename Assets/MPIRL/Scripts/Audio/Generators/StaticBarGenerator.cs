using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBarGenerator : Generator {
  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (beat == (instrument.noteOffset + 8) % 4) {
      double playDspTime = dspTime + System.Math.Abs(0.5 * beatTime * instrument.noteOffset);
      instrument.PlayNote(playDspTime, 0);
    }
  }
}
