﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.


namespace StarkPlatform.Compiler.Shared.Extensions
{
    internal interface ITypeGenerator
    {
        ITypeSymbol CreateArrayTypeSymbol(ITypeSymbol elementType);
        ITypeSymbol CreatePointerTypeSymbol(ITypeSymbol pointedAtType);
        ITypeSymbol Construct(INamedTypeSymbol namedType, ITypeSymbol[] typeArguments);
    }
}
