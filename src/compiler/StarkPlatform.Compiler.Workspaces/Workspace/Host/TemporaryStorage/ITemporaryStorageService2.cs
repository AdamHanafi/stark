﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Text;
using System.Threading;

namespace StarkPlatform.Compiler.Host
{
    /// <summary>
    /// This service allows you to access temporary storage.
    /// </summary>
    internal interface ITemporaryStorageService2 : ITemporaryStorageService
    {
        /// <summary>
        /// Attach to existing <see cref="ITemporaryStreamStorage"/> with given name.
        /// </summary>
        ITemporaryStreamStorage AttachTemporaryStreamStorage(string storageName, long offset, long size, CancellationToken cancellationToken = default);

        /// <summary>
        /// Attach to existing <see cref="ITemporaryTextStorage"/> with given name.
        /// </summary>
        ITemporaryTextStorage AttachTemporaryTextStorage(string storageName, long offset, long size, Encoding encoding, CancellationToken cancellationToken = default);
    }
}
