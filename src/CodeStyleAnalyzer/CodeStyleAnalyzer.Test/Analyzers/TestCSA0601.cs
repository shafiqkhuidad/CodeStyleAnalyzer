﻿using CodeStyleAnalyzer.Analyzers;
using CodeStyleAnalyzer.CodeFixers;
using CodeStyleAnalyzer.Test.Verifiers;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace CodeStyleAnalyzer.Test.Analyzers
{
    public class TestCSA0601 : CodeStyleAnalyzerVerifier
    {
        protected override string CodeRuleId { get; } = "CSA0601";

        protected override string CodeRuleMessage { get; } = "Namespace imports should be specified at the top of the file, outside of namespace declarations";

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new NamespaceAnalyzer();
        protected override CodeFixProvider GetCSharpCodeFixProvider() => new NamespaceCodeFixProvider();

        [Fact]
        public void CorrectFile_ShouldNotGiveError()
        {
            var source = @"
using System;
namespace Test
{
    using TestFunc = Func<int>;
    internal class Test { }
}";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void IncorrectFile_ShouldGiveError()
        {
            var source = @"
namespace Test
{
    using System;
    internal class Test { }
}";
            var expectedSource = @"using System;

namespace Test
{
    internal class Test { }
}";
            var expectedDiagnostic = GetDiagnosticResult(4, 5);
            VerifyCSharpDiagnostic(source, expectedDiagnostic);
            VerifyCSharpFix(source, expectedSource);
        }

        [Fact]
        public void AutogeneratedFile_ShouldNotGiveError()
        {
            var source = @"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------
namespace Test
{
    using System;
    internal class Test { }
}";
            VerifyCSharpDiagnostic(source);
        }
    }
}
