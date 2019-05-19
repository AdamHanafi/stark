﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Roslyn.Utilities;

namespace StarkPlatform.Compiler
{
    /// <summary>
    /// Type layout information.
    /// </summary>
    internal struct TypeLayout : IEquatable<TypeLayout>
    {
        private readonly byte _kind;
        private readonly short _pack;
        private readonly short _alignment;
        private readonly int _size;

        public TypeLayout(LayoutKind kind, int size, byte pack, byte alignment)
        {
            Debug.Assert(size >= 0 && (int)kind >= 0 && (int)kind <= 3);

            // we want LayoutKind.Auto to be the default layout for default(TypeLayout):
            Debug.Assert(LayoutKind.Sequential == 0);
            _kind = (byte)(kind + 1);

            _size = size;
            _pack = pack;
            _alignment = alignment;
        }

        /// <summary>
        /// Layout kind (Layout flags in metadata).
        /// </summary>
        public LayoutKind Kind
        {
            get
            {
                // for convenience default(TypeLayout) should be auto-layout
                return _kind == 0 ? LayoutKind.Auto : (LayoutKind)(_kind - 1);
            }
        }

        /// <summary>
        /// Field packing (PackingSize field in metadata).
        /// </summary>
        public short Pack
        {
            get { return _pack; }
        }

        /// <summary>
        /// Struct alignment (PackingSize field in metadata).
        /// </summary>
        public short Alignment
        {
            get { return _alignment; }
        }

        /// <summary>
        /// Size of the type.
        /// </summary>
        public int Size
        {
            get { return _size; }
        }

        public bool Equals(TypeLayout other)
        {
            return _size == other._size
                && _pack == other._pack
                && _alignment == other._alignment
                && _kind == other._kind;
        }

        public override bool Equals(object obj)
        {
            return obj is TypeLayout && Equals((TypeLayout)obj);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Hash.Combine(Hash.Combine(this.Size, this.Pack), this.Alignment), _kind);
        }
    }
}
