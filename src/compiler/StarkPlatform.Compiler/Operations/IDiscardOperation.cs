﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StarkPlatform.Compiler.Operations
{
    /// <summary>
    /// Represents a discard operation.
    /// <para>
    /// Current Usage: C# discard expressions
    /// </para>
    /// </summary>
    /// <remarks>
    /// This interface is reserved for implementation by its associated APIs. We reserve the right to
    /// change it in the future.
    /// </remarks>
    public interface IDiscardOperation : IOperation
    {
        /// <summary>
        /// The symbol of the discard operation.
        /// </summary>
        IDiscardSymbol DiscardSymbol { get; }
    }
}
