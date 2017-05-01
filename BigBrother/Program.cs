using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.IO;

namespace BigBrother
{
    class Program
    {
        public class ProcessInfo
        {
            const string query = "SELECT Name, CommandLine, ProcessId, Caption, ExecutablePath FROM Win32_Process WHERE ProcessId = ";

            public readonly Process Process;
            public readonly string CommandLine;

            public ProcessInfo(Process process)
            {
                Process = process;
                ManagementObject ManagementObject = new ManagementObjectSearcher(query + process.Id).Get().OfType<ManagementObject>().Single();
                CommandLine = (ManagementObject["CommandLine"] ?? ManagementObject["ExecutablePath"])?.ToString();
            }

            public override string ToString()
            {
                return string.Join(" ", Process.ProcessName, CommandLine);
            }
        }

        public class ProcessComparer : IEqualityComparer<Process>
        {
            public static ProcessComparer Instance = new ProcessComparer();
            public bool Equals(Process x, Process y) { return x.Id == y.Id; }
            public int GetHashCode(Process obj) { return obj.Id; }
        }


        static void Main(string[] args)
        {
            IEnumerable<ProcessInfo> collected = GetNewProcesses(TimeSpan.FromMinutes(1));
            string result = string.Join(Environment.NewLine, collected);
            File.WriteAllText(DateTime.Now.ToFileTime() + ".log", result);
            Console.WriteLine(result);
        }

        static IEnumerable<ProcessInfo> GetNewProcesses(TimeSpan interval)
        {
            DateTime startTime = DateTime.Now;
            IEnumerable<Process> initial = Process.GetProcesses();
            List<ProcessInfo> collected = new List<ProcessInfo>();

            while (DateTime.Now - startTime < interval)
            {
                IEnumerable<Process> Current = Process.GetProcesses().Except(initial.Union(collected.Select(p => p.Process)), ProcessComparer.Instance);
                collected.AddRange(Current.Select(p => new ProcessInfo(p)));
            }
            return collected;
        }
    }
}
