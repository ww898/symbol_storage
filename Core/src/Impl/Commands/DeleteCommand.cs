﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.SymbolStorage.Impl.Logger;
using JetBrains.SymbolStorage.Impl.Storages;

namespace JetBrains.SymbolStorage.Impl.Commands
{
  internal sealed class DeleteCommand
  {
    private readonly ILogger myLogger;
    private readonly IStorage myStorage;
    private readonly IReadOnlyCollection<string> myIncProductWildcards;
    private readonly IReadOnlyCollection<string> myExcProductWildcards;
    private readonly IReadOnlyCollection<string> myIncVersionWildcards;
    private readonly IReadOnlyCollection<string> myExcVersionWildcards;

    public DeleteCommand(
      [NotNull] ILogger logger,
      [NotNull] IStorage storage,
      [NotNull] IReadOnlyCollection<string> incProductWildcards,
      [NotNull] IReadOnlyCollection<string> excProductWildcards,
      [NotNull] IReadOnlyCollection<string> incVersionWildcards,
      [NotNull] IReadOnlyCollection<string> excVersionWildcards)
    {
      myLogger = logger ?? throw new ArgumentNullException(nameof(logger));
      myStorage = storage ?? throw new ArgumentNullException(nameof(storage));
      myIncProductWildcards = incProductWildcards ?? throw new ArgumentNullException(nameof(incProductWildcards));
      myExcProductWildcards = excProductWildcards ?? throw new ArgumentNullException(nameof(excProductWildcards));
      myIncVersionWildcards = incVersionWildcards ?? throw new ArgumentNullException(nameof(incVersionWildcards));
      myExcVersionWildcards = excVersionWildcards ?? throw new ArgumentNullException(nameof(excVersionWildcards));
    }

    public async Task<int> Execute()
    {
      var validator = new Validator(myLogger, myStorage);
      var storageFormat = await validator.ValidateStorageMarkers();

      long deleteTags;
      {
        var tagItems = await validator.LoadTagItems(
          myIncProductWildcards,
          myExcProductWildcards,
          myIncVersionWildcards,
          myExcVersionWildcards);
        validator.DumpProducts(tagItems);
        validator.DumpProperties(tagItems);
        deleteTags = tagItems.Count;

        myLogger.Info($"[{DateTime.Now:s}] Deleting tag files...");
        foreach (var tagItem in tagItems)
        {
          var file = tagItem.Key;
          myLogger.Info($"  Deleting {file}");
          await myStorage.Delete(file);
        }
      }

      {
        var tagItems = await validator.LoadTagItems();
        var (_, files) = await validator.GatherDataFiles();
        var (statistics, deleted) = await validator.Validate(tagItems, files, storageFormat, Validator.ValidateMode.Delete);
        myLogger.Info($"[{DateTime.Now:s}] Done (deleted tag files: {deleteTags}, deleted data files: {deleted}, warnings: {statistics.Warnings}, errors: {statistics.Errors}, fixes: {statistics.Fixes})");
        return statistics.HasProblems ? 1 : 0;
      }
    }
  }
}