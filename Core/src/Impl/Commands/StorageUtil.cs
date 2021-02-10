﻿using System;
using JetBrains.Annotations;
using JetBrains.SymbolStorage.Impl.Storages;

namespace JetBrains.SymbolStorage.Impl.Commands
{
  internal static class StorageUtil
  {
    public const string AwsS3AccessKeyEnvironmentVariable = "JETBRAINS_AWSS3_ACCESS_KEY";
    public const string AwsS3SecretKeyEnvironmentVariable = "JETBRAINS_AWSS3_SECRET_KEY";

    public const string NormalStorageFormat = "normal";
    public const string LowerStorageFormat = "lower";
    public const string UpperStorageFormat = "upper";

    [NotNull]
    public static IStorage GetStorage([CanBeNull] string dir, [CanBeNull] string awsS3BucketName)
    {
      if (!string.IsNullOrEmpty(dir) && string.IsNullOrEmpty(awsS3BucketName))
        return new FileSystemStorage(dir);
      if (string.IsNullOrEmpty(dir) && !string.IsNullOrEmpty(awsS3BucketName))
      {
        var accessKey = Environment.GetEnvironmentVariable(AwsS3AccessKeyEnvironmentVariable) ?? ConsoleUtil.ReadHiddenConsoleInput("Enter AWS S3 access key");
        var secretKey = Environment.GetEnvironmentVariable(AwsS3SecretKeyEnvironmentVariable) ?? ConsoleUtil.ReadHiddenConsoleInput("Enter AWS S3 secret key");
        return new AwsS3Storage(accessKey, secretKey, awsS3BucketName);
      }

      throw new Exception("The storage location option should be defined");
    }

    public static StorageFormat GetStorageFormat([CanBeNull] string casing) => casing switch
      {
        null => StorageFormat.Normal,
        NormalStorageFormat => StorageFormat.Normal,
        LowerStorageFormat => StorageFormat.LowerCase,
        UpperStorageFormat => StorageFormat.UpperCase,
        _ => throw new ArgumentOutOfRangeException(nameof(casing), casing, null)
      };
  }
}