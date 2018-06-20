using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarelyAPI;

public static class HarmonicProgression {
  private static MarkovChain markov = null;

  private static Dictionary<SectionType, int[]> progressions = null;

  public static void Initialize(int numBars) {
    markov = new MarkovChain(8, 1);
    progressions = new Dictionary<SectionType, int[]>();

    for (int i = 0; i < SongStructure.Length(); ++i) {
      var section = SongStructure.GetSection(i);
      if (!progressions.ContainsKey(section)) {
        if (section != SectionType.BRIDGE) {
          markov.Reset();
        }
        int[] progression = new int[numBars];
        for (int bar = 0; bar < numBars; ++bar) {
          progression[bar] = markov.CurrentState;
          markov.GenerateNextState();
        }
        progressions.Add(section, progression);
      }
    }
  }

  public static int GetHarmonic(int section, int barIndex) {
    int[] progression = null;
    if (progressions.TryGetValue(SongStructure.GetSection(section), out progression)) {
      return progression[barIndex];
    }
    return 0;
  }
}
