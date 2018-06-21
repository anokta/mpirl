using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarelyAPI;

public class SimpleBassline : Generator {
  public int lastNote = 0;
  public int beatOffset = 0;

  private Dictionary<SectionType, Dictionary<int, int>> lines;

  // TODO(anokta): Get these from sequencer.
  private readonly int numBeats = 4;

  protected override void Awake() {
    base.Awake();
    lines = new Dictionary<SectionType, Dictionary<int, int>>();
    for (int i = 0; i < SongStructure.Length(); ++i) {
      var section = SongStructure.GetSection(i);
      if (!lines.ContainsKey(section)) {
        lines.Add(section, new Dictionary<int, int>());
      }
    }
  }

  protected override void OnNextBeat(int section, int bar, int beat, double dspTime, double beatTime) {
    if (beat == beatOffset) {
      lastNote += RandomNumber.NextInt(-Scale.scaleLength / 4, Scale.scaleLength / 2);
      lastNote = System.Math.Max(-Scale.scaleLength / 4, System.Math.Min(Scale.scaleLength, lastNote));

      Dictionary<int, int> sectionLines = null;
      if (lines.TryGetValue(SongStructure.GetSection(section), out sectionLines)) {
        int key = bar * numBeats + beat;
        int noteIndex = 0;
        if (!sectionLines.TryGetValue(key, out noteIndex)) {
          noteIndex = HarmonicProgression.GetHarmonic(section, bar) + lastNote;
          sectionLines.Add(key, noteIndex);
        }
        instrument.PlayNote(dspTime, noteIndex, RandomNumber.NextFloat(0.9f, 1.0f));
      }
    }
  }
}
