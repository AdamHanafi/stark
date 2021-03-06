﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.Compiler.CodeFixes;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Formatting;
using StarkPlatform.Compiler.Formatting.Rules;
using StarkPlatform.Compiler.Operations;
using StarkPlatform.Compiler.Simplification;
using StarkPlatform.Compiler.UseConditionalExpression;

namespace StarkPlatform.Compiler.Stark.UseConditionalExpression
{
    [ExportCodeFixProvider(LanguageNames.Stark), Shared]
    internal partial class CSharpUseConditionalExpressionForAssignmentCodeRefactoringProvider
        : AbstractUseConditionalExpressionForAssignmentCodeFixProvider<
            StatementSyntax, IfStatementSyntax, LocalDeclarationStatementSyntax, VariableDeclarationSyntax, ExpressionSyntax, IfExpressionSyntax>
    {
        protected override IFormattingRule GetMultiLineFormattingRule()
            => MultiLineConditionalExpressionFormattingRule.Instance;

        protected override VariableDeclarationSyntax WithInitializer(VariableDeclarationSyntax variable, ExpressionSyntax value)
            => variable.WithInitializer(SyntaxFactory.EqualsValueClause(value));

        protected override VariableDeclarationSyntax GetDeclaratorSyntax(IVariableDeclarationOperation declarator)
            => (VariableDeclarationSyntax)declarator.Syntax;

        protected override LocalDeclarationStatementSyntax AddSimplificationToType(LocalDeclarationStatementSyntax statement)
            => statement.WithDeclaration(statement.Declaration.WithType(
                statement.Declaration.Type.WithAdditionalAnnotations(Simplifier.Annotation)));

        protected override StatementSyntax WrapWithBlockIfAppropriate(
            IfStatementSyntax ifStatement, StatementSyntax statement)
        {
            if (ifStatement.Parent is ElseClauseSyntax &&
                ifStatement.Statement is BlockSyntax block)
            {
                return block.WithStatements(SyntaxFactory.SingletonList(statement))
                            .WithAdditionalAnnotations(Formatter.Annotation);
            }

            return statement;
        }
    }
}
