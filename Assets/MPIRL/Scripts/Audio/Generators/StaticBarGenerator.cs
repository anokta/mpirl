using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBarGenerator : Generator {
  public AudioSource source;

  public int noteOffset = 0;

  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (beat == (noteOffset + 8) % 4) {
      PlayNote(0, dspTime + System.Math.Abs(0.5 * beatTime * noteOffset));
    }
  }

  protected override void PlayNote(int noteIndex, double dspTime) {
    int octaveShift = Mathf.FloorToInt((float)(noteIndex + noteOffset) / 8.0f);
    int offset = (noteIndex + noteOffset + 32) % Scale.majorScale.Length;
    source.pitch = Mathf.Pow(2.0f, octaveShift) * Scale.majorScale[offset];

    source.PlayScheduled(dspTime);
  }
}
