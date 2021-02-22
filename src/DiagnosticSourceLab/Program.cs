using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiagnosticSourceLab
{
    public class Program
    {
        // Fields
        private static DiagnosticSource _diagnosticSource = new DiagnosticListener("CLK.DiagnosticSourceLab");


        // Methods
        public static void Main(string[] args)
        {
            // Subscribe
            DiagnosticListener.AllListeners.Subscribe(new DiagnosticSourceSubscriber("CLK.DiagnosticSourceLab"));

            // Diagnostic
            {
                // Pay
                var activityName01 = "Pay";
                if (_diagnosticSource.IsEnabled(activityName01) == true)
                {
                    _diagnosticSource.Write(activityName01, new { User = "Clark" });
                }

                // Print
                var activityName02 = "Print";
                if (_diagnosticSource.IsEnabled(activityName02) == true)
                {
                    _diagnosticSource.Write(activityName02, new { User = "Jane" });
                }
            }
        }


        // Class
        public class DiagnosticSourceSubscriber : IObserver<DiagnosticListener>
        {
            // Fields
            private readonly string _diagnosticSourceName = null;

            private readonly DiagnosticExporter _diagnosticExporter = new DiagnosticExporter();


            // Constructors
            public DiagnosticSourceSubscriber(string diagnosticSourceName)
            {
                #region Contracts

                if (string.IsNullOrEmpty(diagnosticSourceName) == true) throw new ArgumentException(nameof(diagnosticSourceName));

                #endregion

                // Default
                _diagnosticSourceName = diagnosticSourceName;
            }


            // Methods
            public void OnNext(DiagnosticListener diagnosticSource)
            {
                #region Contracts

                if (diagnosticSource == null) throw new ArgumentException(nameof(diagnosticSource));

                #endregion

                // Require
                if (diagnosticSource.Name != _diagnosticSourceName) return;

                // Subscribe
                diagnosticSource.Subscribe(_diagnosticExporter);
            }

            public void OnCompleted() { }

            public void OnError(Exception error) { }
        }

        public class DiagnosticExporter : IObserver<KeyValuePair<string, object>>
        {
            // Methods
            public void OnNext(KeyValuePair<string, object> diagnostic)
            {
                // Display
                Console.WriteLine($"Activity: Name={diagnostic.Key}, Printload={diagnostic.Value}");
            }

            public void OnCompleted() { }

            public void OnError(Exception error) { }
        }
    }
}
