﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace StarkPlatform.Compiler.Shared.Utilities
{
    internal static class IOUtilities
    {
        public static void PerformIO(Action action)
        {
            PerformIO<object>(() =>
            {
                action();
                return null;
            });
        }

        public static T PerformIO<T>(Func<T> function, T defaultValue = default)
        {
            try
            {
                return function();
            }
            catch (Exception e) when (IsNormalIOException(e))
            {
            }

            return defaultValue;
        }

        public static async Task<T> PerformIOAsync<T>(Func<Task<T>> function, T defaultValue = default)
        {
            try
            {
                return await function().ConfigureAwait(false);
            }
            catch (Exception e) when (IsNormalIOException(e))
            {
            }

            return defaultValue;
        }

        public static bool IsNormalIOException(Exception e)
        {
            return e is IOException ||
                   e is SecurityException ||
                   e is ArgumentException ||
                   e is UnauthorizedAccessException ||
                   e is NotSupportedException ||
                   e is InvalidOperationException;
        }
    }
}
