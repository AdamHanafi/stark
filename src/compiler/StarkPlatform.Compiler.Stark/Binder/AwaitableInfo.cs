﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.Compiler.Stark.Symbols;

namespace StarkPlatform.Compiler.Stark
{
    /// <summary>
    /// Internal structure containing all semantic information about an await expression.
    /// </summary>
    internal sealed class AwaitableInfo
    {
        public static readonly AwaitableInfo Empty = new AwaitableInfo(getAwaiterMethod: null, isCompletedProperty: null, getResultMethod: null);

        public readonly MethodSymbol GetAwaiter;
        public readonly PropertySymbol IsCompleted;
        public readonly MethodSymbol GetResult;

        internal AwaitableInfo(MethodSymbol getAwaiterMethod, PropertySymbol isCompletedProperty, MethodSymbol getResultMethod)
        {
            this.GetAwaiter = getAwaiterMethod;
            this.IsCompleted = isCompletedProperty;
            this.GetResult = getResultMethod;
        }

        internal AwaitableInfo Update(MethodSymbol newGetAwaiter, PropertySymbol newIsCompleted, MethodSymbol newGetResult)
        {
            if (ReferenceEquals(GetAwaiter, newGetAwaiter) && ReferenceEquals(IsCompleted, newIsCompleted) && ReferenceEquals(GetResult, newGetResult))
            {
                return this;
            }

            return new AwaitableInfo(newGetAwaiter, newIsCompleted, newGetResult);
        }
    }
}
