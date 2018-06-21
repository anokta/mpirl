using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarelyAPI;

public enum DrumType {
  KICK = 0,
  SNARE,
  CLAP,
  HH_CLOSED,
  HH_OPEN
}

public class Drums : Generator {
  public DrumType drumType;

  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (SongStructure.GetSection(section) == SectionType.OUTRO && drumType != DrumType.CLAP) {
      return;
    }
    int augmentedBeat = 2 * (bar * 4 + beat);
    for (int i = 0; i < 2; ++i) {
      float p = RandomNumber.NextFloat(0.0f, 1.0f);
      bool play = false;
      switch (drumType) {
        case DrumType.KICK:
          play = p < 0.0025f || (p < 0.96f && augmentedBeat % 8 == 0);
          break;
        case DrumType.SNARE:
          play = p < 0.0025f || augmentedBeat % 8 == 4;
          break;
        case DrumType.CLAP:
          play = p < 0.0025f || augmentedBeat % 8 == 4;
          break;
        case DrumType.HH_CLOSED:
          play = p < 0.001f || augmentedBeat % 4 == 2;
          break;
        case DrumType.HH_OPEN:
          play = augmentedBeat % 32 == 0;
          break;
      }
      if (play) {
        double playTime = dspTime + i * 0.5 * beatTime;
        instrument.PlayNote(playTime, 0);
      }
      ++augmentedBeat;
    }
  }
}
