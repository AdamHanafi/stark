﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using StarkPlatform.Compiler.Options;
using StarkPlatform.Compiler.Options.Providers;

namespace StarkPlatform.Compiler.SolutionCrawler
{
    [ExportOptionProvider, Shared]
    internal class InternalSolutionCrawlerOptionsProvider : IOptionProvider
    {
        public ImmutableArray<IOption> Options { get; } = ImmutableArray.Create<IOption>(
            InternalSolutionCrawlerOptions.SolutionCrawler,
            InternalSolutionCrawlerOptions.ActiveFileWorkerBackOffTimeSpanInMS,
            InternalSolutionCrawlerOptions.AllFilesWorkerBackOffTimeSpanInMS,
            InternalSolutionCrawlerOptions.EntireProjectWorkerBackOffTimeSpanInMS,
            InternalSolutionCrawlerOptions.SemanticChangeBackOffTimeSpanInMS,
            InternalSolutionCrawlerOptions.ProjectPropagationBackOffTimeSpanInMS,
            InternalSolutionCrawlerOptions.PreviewBackOffTimeSpanInMS);
    }
}
