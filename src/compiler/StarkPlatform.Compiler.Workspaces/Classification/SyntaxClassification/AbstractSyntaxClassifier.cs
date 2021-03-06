﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using StarkPlatform.Compiler.PooledObjects;
using StarkPlatform.Compiler.Text;

namespace StarkPlatform.Compiler.Classification.Classifiers
{
    internal abstract class AbstractSyntaxClassifier : ISyntaxClassifier
    {
        protected AbstractSyntaxClassifier()
        {
        }

        protected string GetClassificationForType(ITypeSymbol type)
            => type.GetClassification();

        public virtual void AddClassifications(Workspace workspace, SyntaxNode syntax, SemanticModel semanticModel, ArrayBuilder<ClassifiedSpan> result, CancellationToken cancellationToken)
        {
        }

        public virtual void AddClassifications(Workspace workspace, SyntaxToken syntax, SemanticModel semanticModel, ArrayBuilder<ClassifiedSpan> result, CancellationToken cancellationToken)
        {
        }

        public virtual ImmutableArray<Type> SyntaxNodeTypes
            => ImmutableArray<Type>.Empty;

        public virtual ImmutableArray<int> SyntaxTokenKinds
            => ImmutableArray<int>.Empty;
    }
}
