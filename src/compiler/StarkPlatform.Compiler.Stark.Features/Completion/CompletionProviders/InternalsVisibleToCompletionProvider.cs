﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using StarkPlatform.Compiler.Completion.Providers;
using StarkPlatform.Compiler.Stark.Syntax;

namespace StarkPlatform.Compiler.Stark.Completion.Providers
{
    internal sealed class InternalsVisibleToCompletionProvider : AbstractInternalsVisibleToCompletionProvider
    {

        protected override IImmutableList<SyntaxNode> GetAssemblyScopedAttributeSyntaxNodesOfDocument(SyntaxNode documentRoot)
        {
            var builder = default(ImmutableList<SyntaxNode>.Builder);
            if (documentRoot is CompilationUnitSyntax compilationUnit)
            {
                foreach (var attribute in compilationUnit.AttributeLists)
                {
                    // For most documents the compilationUnit.AttributeLists should be empty.
                    // Therefore we delay initialization of the builder
                    builder = builder ?? ImmutableList.CreateBuilder<SyntaxNode>();
                    builder.Add(attribute);
                }
            }

            return builder == null
                ? ImmutableList<SyntaxNode>.Empty
                : builder.ToImmutable();
        }

        protected override SyntaxNode GetConstructorArgumentOfInternalsVisibleToAttribute(SyntaxNode internalsVisibleToAttribute)
        {
            var arguments = ((AttributeSyntax)internalsVisibleToAttribute).ArgumentList.Arguments;
            // InternalsVisibleTo has only one constructor argument. 
            // https://msdn.microsoft.com/en-us/library/system.runtime.compilerservices.internalsvisibletoattribute.internalsvisibletoattribute(v=vs.110).aspx
            // We can assume that this is the assemblyName argument.
            return arguments.Count > 0
                ? arguments[0].Expression
                : null;
        }
    }
}
