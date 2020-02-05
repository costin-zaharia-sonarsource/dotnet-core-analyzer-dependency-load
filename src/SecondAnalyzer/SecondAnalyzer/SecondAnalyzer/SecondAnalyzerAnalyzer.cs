using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Threading;
using Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SecondAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SecondAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SecondAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        private const string LogFilePath = "debug-output.txt";

        public SecondAnalyzerAnalyzer()
        {
            File.AppendAllText(LogFilePath, "Second assembly: Register to events\n");

            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            File.AppendAllText(LogFilePath, $"Second assembly resolve - Name: {args.Name}, Requesting Assembly: {args.RequestingAssembly}\n");
            return null;
        }

        private static void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            File.AppendAllText(LogFilePath, $"Second assembly loaded: {args.LoadedAssembly}\n");
        }

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(c => new Dummy().LogMessage(), SymbolKind.NamedType);
    }
}
