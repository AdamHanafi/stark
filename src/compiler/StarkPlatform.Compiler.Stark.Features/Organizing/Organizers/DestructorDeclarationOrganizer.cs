﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using StarkPlatform.Compiler.Stark.Syntax;
using StarkPlatform.Compiler.Organizing.Organizers;

namespace StarkPlatform.Compiler.Stark.Organizing.Organizers
{
    [ExportSyntaxNodeOrganizer(LanguageNames.Stark), Shared]
    internal class DestructorDeclarationOrganizer : AbstractSyntaxNodeOrganizer<DestructorDeclarationSyntax>
    {
        protected override DestructorDeclarationSyntax Organize(
            DestructorDeclarationSyntax syntax,
            CancellationToken cancellationToken)
        {
            return syntax.Update(syntax.AttributeLists,
                ModifiersOrganizer.Organize(syntax.Modifiers),
                syntax.TildeToken,
                syntax.Identifier,
                syntax.ParameterList,
                syntax.ContractClauses,
                syntax.Body,
                syntax.EosToken);
        }
    }
}
