﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Options;
using StarkPlatform.Compiler.PooledObjects;
using StarkPlatform.Compiler.Structure;

namespace StarkPlatform.Compiler.Stark.Structure
{
    internal class EventFieldDeclarationStructureProvider : AbstractSyntaxNodeStructureProvider<EventFieldDeclarationSyntax>
    {
        protected override void CollectBlockSpans(
            EventFieldDeclarationSyntax eventFieldDeclaration,
            ArrayBuilder<BlockSpan> spans,
            OptionSet options,
            CancellationToken cancellationToken)
        {
            CSharpStructureHelpers.CollectCommentBlockSpans(eventFieldDeclaration, spans);
        }
    }
}
