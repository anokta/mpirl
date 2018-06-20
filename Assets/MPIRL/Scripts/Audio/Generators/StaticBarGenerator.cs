using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBarGenerator : Generator {
  public Instrument instrument;

  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (beat == (instrument.noteOffset + 8) % 4) {
      PlayNote(0, dspTime + System.Math.Abs(0.5 * beatTime * instrument.noteOffset));
    }
  }

  protected override void PlayNote(int noteIndex, double dspTime) {
    instrument.PlayNote(dspTime, noteIndex);
  }
}
