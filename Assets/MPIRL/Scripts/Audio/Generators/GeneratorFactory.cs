﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratorFactory {
  public static Generator CreateGenerator(Instrument instrument, string generatorName) {
    Type generatorType = Type.GetType(generatorName, true, true);
    var generator = instrument.gameObject.AddComponent(generatorType) as Generator;
    return generator;
  }
}
