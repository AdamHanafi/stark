﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using StarkPlatform.Compiler.CodeRefactorings;
using StarkPlatform.Compiler.ConvertToInterpolatedString;
using StarkPlatform.Compiler.Stark.Syntax;

namespace StarkPlatform.Compiler.Stark.ConvertToInterpolatedString
{
    [ExportCodeRefactoringProvider(LanguageNames.Stark, Name = PredefinedCodeRefactoringProviderNames.ConvertToInterpolatedString), Shared]
    internal partial class CSharpConvertPlaceholderToInterpolatedStringRefactoringProvider :
        AbstractConvertPlaceholderToInterpolatedStringRefactoringProvider<InvocationExpressionSyntax, ExpressionSyntax, ArgumentSyntax, LiteralExpressionSyntax>
    {
        protected override SyntaxNode GetInterpolatedString(string text)
            => SyntaxFactory.ParseExpression("$" + text) as InterpolatedStringExpressionSyntax;
    }
}
