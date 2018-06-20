using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BarelyAPI;

public enum SectionType { INTRO = 'I', VERSE = 'V', PRE_CHORUS = 'P', CHORUS = 'C', BRIDGE = 'B', OUTRO = 'O', END = '.', NONE = ' ' } 

public static class SongStructure {
  private static string sectionSequence;

  public static int Length() {
    return sectionSequence.Length;
  }
  
  public static void Initialize() {
    var grammar = new ContextFreeGrammar();
    grammar.AddRule("Start", "Intro Body Outro");
    grammar.AddRule("Intro", "I");
    grammar.AddRule("Body", "Statement Repetition Cadence");
    grammar.AddRule("Statement", "V V V P C C | V V V V C C | V V C");
    grammar.AddRule("Repetition", "V P C C | V V C C | V P C | Repetition Repetition | Repetition B Repetition");
    grammar.AddRule("Cadence", "C | C C | P C C");
    grammar.AddRule("Outro", "O");
    sectionSequence = grammar.GenerateSequence("Start");

    Debug.Log(sectionSequence);
  }

  public static SectionType GetSection(int sectionIndex) {
    return (SectionType)sectionSequence[sectionIndex % sectionSequence.Length];
  }
}
