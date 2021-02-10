﻿using JetBrains.Annotations;

namespace JetBrains.SymbolStorage.Impl.Logger
{
  internal interface ILogger
  {
    void Info([NotNull] string str);
    void Fix([NotNull] string str);
    void Warning([NotNull] string str);
    void Error([NotNull] string str);
  }
}