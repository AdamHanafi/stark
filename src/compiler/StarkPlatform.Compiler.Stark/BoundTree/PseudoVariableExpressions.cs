﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StarkPlatform.Compiler.Stark
{
    /// <summary>
    /// BoundExpressions to be used for emit. The expressions are assumed
    /// to be lowered and will not be visited by <see cref="BoundTreeWalker"/>.
    /// </summary>
    internal abstract class PseudoVariableExpressions
    {
        internal abstract BoundExpression GetValue(BoundPseudoVariable variable, DiagnosticBag diagnostics);
        internal abstract BoundExpression GetAddress(BoundPseudoVariable variable);
    }
}
