// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using ClangSharp.Interop;

namespace ClangSharp;

public sealed class FunctionProtoType : FunctionType
{
    private readonly Lazy<IReadOnlyList<Type>> _paramTypes;

    internal FunctionProtoType(CXType handle) : base(handle, CXTypeKind.CXType_FunctionProto, CX_TypeClass.CX_TypeClass_FunctionProto)
    {
        _paramTypes = new Lazy<IReadOnlyList<Type>>(() => {
            var paramTypeCount = Handle.NumArgTypes;
            var paramTypes = new List<Type>(paramTypeCount);

            for (var i = 0; i < paramTypeCount; i++)
            {
                var paramType = TranslationUnit.GetOrCreate<Type>(Handle.GetArgType(unchecked((uint)i)));
                paramTypes.Add(paramType);
            }

            return paramTypes;
        });
    }

    public CXCursor_ExceptionSpecificationKind ExceptionSpecType => Handle.ExceptionSpecificationType;

    public bool IsVariadic => Handle.IsFunctionTypeVariadic;

    public uint NumParams => (uint)Handle.NumArgTypes;

    public IReadOnlyList<Type> ParamTypes => _paramTypes.Value;

    public CXRefQualifierKind RefQualifier => Handle.CXXRefQualifier;
}
