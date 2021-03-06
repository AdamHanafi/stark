﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using StarkPlatform.Compiler.Stark.Symbols;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Text;

namespace StarkPlatform.Compiler.Stark.Syntax
{
    public partial class ExpressionStatementSyntax
    {
        /// <summary>
        /// Returns true if the <see cref="Expression"/> property is allowed by the rules of the
        /// language to be an arbitrary expression, not just a statement expression.
        /// </summary>
        /// <remarks>
        /// True if, for example, this expression statement represents the last expression statement
        /// of the interactive top-level code.
        /// </remarks>
        public bool AllowsAnyExpression
        {
            get
            {
                var semicolon = EosToken;
                return semicolon.IsMissing && !semicolon.ContainsDiagnostics;
            }
        }
    }
}
