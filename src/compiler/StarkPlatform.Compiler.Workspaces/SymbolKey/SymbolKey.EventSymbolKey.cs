﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using Roslyn.Utilities;

namespace StarkPlatform.Compiler
{
    internal partial struct SymbolKey
    {
        private static class EventSymbolKey
        {
            public static void Create(IEventSymbol symbol, SymbolKeyWriter visitor)
            {
                visitor.WriteString(symbol.MetadataName);
                visitor.WriteSymbolKey(symbol.ContainingType);
            }

            public static SymbolKeyResolution Resolve(SymbolKeyReader reader)
            {
                var metadataName = reader.ReadString();
                var containingTypeResolution = reader.ReadSymbolKey();

                var events = GetAllSymbols<INamedTypeSymbol>(containingTypeResolution)
                    .SelectMany(t => t.GetMembers(metadataName)).OfType<IEventSymbol>();
                return CreateSymbolInfo(events);
            }
        }
    }
}
