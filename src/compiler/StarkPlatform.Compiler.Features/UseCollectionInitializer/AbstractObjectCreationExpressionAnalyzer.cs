﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using StarkPlatform.Compiler.LanguageServices;
using StarkPlatform.Compiler.PooledObjects;
using StarkPlatform.Compiler.Shared.Extensions;

namespace StarkPlatform.Compiler.UseCollectionInitializer
{
    internal abstract class AbstractObjectCreationExpressionAnalyzer<
        TExpressionSyntax,
        TStatementSyntax,
        TObjectCreationExpressionSyntax,
        TVariableDeclarationSyntax,
        TMatch>
        where TExpressionSyntax : SyntaxNode
        where TStatementSyntax : SyntaxNode
        where TObjectCreationExpressionSyntax : TExpressionSyntax
        where TVariableDeclarationSyntax : SyntaxNode
    {
        protected SemanticModel _semanticModel;
        protected ISyntaxFactsService _syntaxFacts;
        protected TObjectCreationExpressionSyntax _objectCreationExpression;
        protected CancellationToken _cancellationToken;

        protected TStatementSyntax _containingStatement;
        private SyntaxNodeOrToken _valuePattern;
        private ISymbol _initializedSymbol;

        protected AbstractObjectCreationExpressionAnalyzer()
        {
        }

        public void Initialize(
            SemanticModel semanticModel,
            ISyntaxFactsService syntaxFacts,
            TObjectCreationExpressionSyntax objectCreationExpression,
            CancellationToken cancellationToken)
        {
            _semanticModel = semanticModel;
            _syntaxFacts = syntaxFacts;
            _objectCreationExpression = objectCreationExpression;
            _cancellationToken = cancellationToken;
        }

        protected void Clear()
        {
            _semanticModel = null;
            _syntaxFacts = null;
            _objectCreationExpression = null;
            _cancellationToken = default;
            _containingStatement = null;
            _valuePattern = default;
            _initializedSymbol = null;
        }

        protected abstract void AddMatches(ArrayBuilder<TMatch> matches);

        protected ImmutableArray<TMatch>? AnalyzeWorker()
        {
            if (_syntaxFacts.GetObjectCreationInitializer(_objectCreationExpression) != null)
            {
                // Don't bother if this already has an initializer.
                return null;
            }

            if (!ShouldAnalyze())
            {
                return null;
            }

            _containingStatement = _objectCreationExpression.FirstAncestorOrSelf<TStatementSyntax>();
            if (_containingStatement == null)
            {
                return null;
            }

            if (!TryInitializeVariableDeclarationCase() &&
                !TryInitializeAssignmentCase())
            {
                return null;
            }

            var matches = ArrayBuilder<TMatch>.GetInstance();
            AddMatches(matches);
            return matches.ToImmutableAndFree();
        }

        private bool TryInitializeVariableDeclarationCase()
        {
            if (!_syntaxFacts.IsLocalDeclarationStatement(_containingStatement))
            {
                return false;
            }

            var containingDeclarator = _objectCreationExpression.Parent.Parent as TVariableDeclarationSyntax;
            if (containingDeclarator == null)
            {
                return false;
            }

            _initializedSymbol = _semanticModel.GetDeclaredSymbol(containingDeclarator, _cancellationToken);
            if (!_syntaxFacts.IsDeclaratorOfLocalDeclarationStatement(containingDeclarator, _containingStatement))
            {
                return false;
            }

            _valuePattern = _syntaxFacts.GetIdentifierOfVariableDeclaration(containingDeclarator);
            return true;
        }

        private bool TryInitializeAssignmentCase()
        {
            if (!_syntaxFacts.IsSimpleAssignmentStatement(_containingStatement))
            {
                return false;
            }

            _syntaxFacts.GetPartsOfAssignmentStatement(_containingStatement,
                out var left, out var right);
            if (right != _objectCreationExpression)
            {
                return false;
            }

            var typeInfo = _semanticModel.GetTypeInfo(left, _cancellationToken);
            _valuePattern = left;
            _initializedSymbol = _semanticModel.GetSymbolInfo(left, _cancellationToken).GetAnySymbol();
            return true;
        }

        protected bool ValuePatternMatches(TExpressionSyntax expression)
        {
            if (_valuePattern.IsToken)
            {
                return _syntaxFacts.IsIdentifierName(expression) &&
                    _syntaxFacts.AreEquivalent(
                        _valuePattern.AsToken(),
                        _syntaxFacts.GetIdentifierOfSimpleName(expression));
            }
            else
            {
                return _syntaxFacts.AreEquivalent(
                    _valuePattern.AsNode(), expression);
            }
        }

        protected bool ExpressionContainsValuePatternOrReferencesInitializedSymbol(SyntaxNode expression)
        {
            foreach (var subExpression in expression.DescendantNodesAndSelf().OfType<TExpressionSyntax>())
            {
                if (!_syntaxFacts.IsNameOfMemberAccessExpression(subExpression))
                {
                    if (ValuePatternMatches(subExpression))
                    {
                        return true;
                    }
                }

                if (_initializedSymbol != null &&
                    _initializedSymbol.Equals(
                        _semanticModel.GetSymbolInfo(subExpression, _cancellationToken).GetAnySymbol()))
                {
                    return true;
                }
            }

            return false;
        }

        protected abstract bool ShouldAnalyze();
    }
}
