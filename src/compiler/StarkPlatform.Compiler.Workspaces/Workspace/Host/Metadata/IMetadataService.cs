﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StarkPlatform.Compiler.Host
{
    internal interface IMetadataService : IWorkspaceService
    {
        PortableExecutableReference GetReference(string resolvedPath, MetadataReferenceProperties properties);
    }
}
