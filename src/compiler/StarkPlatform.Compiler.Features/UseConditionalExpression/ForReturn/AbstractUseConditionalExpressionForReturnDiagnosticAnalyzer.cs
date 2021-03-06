﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.Compiler.CodeStyle;
using StarkPlatform.Compiler.Diagnostics;
using StarkPlatform.Compiler.Operations;

namespace StarkPlatform.Compiler.UseConditionalExpression
{
    internal abstract class AbstractUseConditionalExpressionForReturnDiagnosticAnalyzer<
        TIfStatementSyntax>
        : AbstractUseConditionalExpressionDiagnosticAnalyzer<TIfStatementSyntax>
        where TIfStatementSyntax : SyntaxNode
    {
        protected AbstractUseConditionalExpressionForReturnDiagnosticAnalyzer(
            LocalizableResourceString message)
            : base(IDEDiagnosticIds.UseConditionalExpressionForReturnDiagnosticId,
                   message,
                   CodeStyleOptions.PreferConditionalExpressionOverReturn)
        {
        }

        protected override bool TryMatchPattern(IConditionalOperation ifOperation)
            => UseConditionalExpressionForReturnHelpers.TryMatchPattern(
                    GetSyntaxFactsService(), ifOperation, out _, out _);
    }
}
