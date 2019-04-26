﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using StarkPlatform.Compiler.Stark;
using StarkPlatform.Compiler.Stark.Symbols;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Text;

namespace StarkPlatform.Compiler.Stark.Extensions
{
    internal static partial class DirectiveSyntaxExtensions
    {
        private static readonly ConditionalWeakTable<SyntaxNode, DirectiveInfo> s_rootToDirectiveInfo =
            new ConditionalWeakTable<SyntaxNode, DirectiveInfo>();

        private static SyntaxNode GetAbsoluteRoot(this SyntaxNode node)
        {
            while (node != null && (node.Parent != null || node is StructuredTriviaSyntax))
            {
                if (node.Parent != null)
                {
                    node = node.Parent;
                }
                else
                {
                    node = node.ParentTrivia.Token.Parent;
                }
            }

            return node;
        }

        private static DirectiveInfo GetDirectiveInfo(SyntaxNode node, CancellationToken cancellationToken)
        {
            var root = node.GetAbsoluteRoot();
            var info = s_rootToDirectiveInfo.GetValue(root, r =>
            {
                var directiveMap = new Dictionary<DirectiveTriviaSyntax, DirectiveTriviaSyntax>(
                    DirectiveSyntaxEqualityComparer.Instance);
                var conditionalMap = new Dictionary<DirectiveTriviaSyntax, IReadOnlyList<DirectiveTriviaSyntax>>(
                    DirectiveSyntaxEqualityComparer.Instance);

                var walker = new DirectiveWalker(directiveMap, conditionalMap, cancellationToken);
                walker.Visit(r);
                walker.Finish();

                return new DirectiveInfo(directiveMap, conditionalMap, inactiveRegionLines: null);
            });

            return info;
        }

        internal static DirectiveTriviaSyntax GetMatchingDirective(this DirectiveTriviaSyntax directive, CancellationToken cancellationToken)
        {
            if (directive == null)
            {
                throw new ArgumentNullException(nameof(directive));
            }

            var directiveSyntaxMap = GetDirectiveInfo(directive, cancellationToken).DirectiveMap;
            directiveSyntaxMap.TryGetValue(directive, out var result);

            return result;
        }

        internal static IReadOnlyList<DirectiveTriviaSyntax> GetMatchingConditionalDirectives(this DirectiveTriviaSyntax directive, CancellationToken cancellationToken)
        {
            if (directive == null)
            {
                throw new ArgumentNullException(nameof(directive));
            }

            var directiveConditionalMap = GetDirectiveInfo(directive, cancellationToken).ConditionalMap;
            directiveConditionalMap.TryGetValue(directive, out var result);

            return result;
        }
    }
}