﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using StarkPlatform.Compiler.CodeGeneration;
using StarkPlatform.Compiler.Stark.Extensions;
using StarkPlatform.Compiler.Stark.Syntax;

namespace StarkPlatform.Compiler.Stark.CodeGeneration
{
    internal static class TypeParameterGenerator
    {
        public static TypeParameterListSyntax GenerateTypeParameterList(
            ImmutableArray<ITypeParameterSymbol> typeParameters, CodeGenerationOptions options)
        {
            return typeParameters.Length == 0
                ? null
                : SyntaxFactory.TypeParameterList(
                    SyntaxFactory.SeparatedList(typeParameters.Select(t => GenerateTypeParameter(t, options))));
        }

        private static TypeParameterSyntax GenerateTypeParameter(ITypeParameterSymbol symbol, CodeGenerationOptions options)
        {
            var varianceKeyword =
                symbol.Variance == VarianceKind.In ? SyntaxFactory.Token(SyntaxKind.InKeyword) :
                symbol.Variance == VarianceKind.Out ? SyntaxFactory.Token(SyntaxKind.OutKeyword) : default;

            return SyntaxFactory.TypeParameter(
                AttributeGenerator.GenerateAttributeLists(symbol.GetAttributes(), options),
                varianceKeyword,
                symbol.Name.ToIdentifierToken());
        }
    }
}
