﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using StarkPlatform.Compiler.CodeFixes;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Editing;
using StarkPlatform.Compiler.RemoveUnusedParametersAndValues;

namespace StarkPlatform.Compiler.Stark.RemoveUnusedParametersAndValues
{
    [ExportCodeFixProvider(LanguageNames.Stark, Name = PredefinedCodeFixProviderNames.RemoveUnusedValues), Shared]
    [ExtensionOrder(After = PredefinedCodeFixProviderNames.AddImport)]
    internal class CSharpRemoveUnusedValuesCodeFixProvider :
        AbstractRemoveUnusedValuesCodeFixProvider<ExpressionSyntax, StatementSyntax, BlockSyntax,
            ExpressionStatementSyntax, LocalDeclarationStatementSyntax, VariableDeclarationSyntax,
            ForStatementSyntax, SwitchSectionSyntax, SwitchLabelSyntax, CatchClauseSyntax, CatchClauseSyntax>
    {
        protected override BlockSyntax WrapWithBlockIfNecessary(IEnumerable<StatementSyntax> statements)
            => SyntaxFactory.Block(statements);

        protected override SyntaxToken GetForEachStatementIdentifier(ForStatementSyntax node)
            => node.Variable.GetFirstToken();

        protected override SyntaxNode TryUpdateNameForFlaggedNode(SyntaxNode node, SyntaxToken newName)
        {
            switch (node.Kind())
            {
                case SyntaxKind.IdentifierName:
                    var identifierName = (IdentifierNameSyntax)node;
                    return identifierName.WithIdentifier(newName.WithTriviaFrom(identifierName.Identifier));

                case SyntaxKind.VariableDeclaration:
                    var variableDeclarator = (VariableDeclarationSyntax)node;
                    return variableDeclarator.WithIdentifier(newName.WithTriviaFrom(variableDeclarator.Identifier));

                case SyntaxKind.SingleVariableDesignation:
                    return newName.ValueText == AbstractRemoveUnusedParametersAndValuesDiagnosticAnalyzer.DiscardVariableName
                        ? SyntaxFactory.DiscardDesignation().WithTriviaFrom(node)
                        : (SyntaxNode)SyntaxFactory.SingleVariableDesignation(newName).WithTriviaFrom(node);

                case SyntaxKind.CatchDeclaration:
                    var catchDeclaration = (CatchDeclarationSyntax)node;
                    return catchDeclaration.WithIdentifier(newName.WithTriviaFrom(catchDeclaration.Identifier));

                default:
                    Debug.Fail($"Unexpected node kind for local/parameter declaration or reference: '{node.Kind()}'");
                    return null;
            }
        }

        protected override void InsertAtStartOfSwitchCaseBlockForDeclarationInCaseLabelOrClause(SwitchSectionSyntax switchCaseBlock, SyntaxEditor editor, LocalDeclarationStatementSyntax declarationStatement)
        {
            var firstStatement = switchCaseBlock.Statements.FirstOrDefault();
            if (firstStatement != null)
            {
                editor.InsertBefore(firstStatement, declarationStatement);
            }
            else
            {
                // Switch section without any statements is an error case.
                // Insert before containing switch statement.
                editor.InsertBefore(switchCaseBlock.Parent, declarationStatement);
            }
        }
    }
}
