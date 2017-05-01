using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BigBrother
{
    class Program
    {
        public class ProcessComparer : IEqualityComparer<Process>
        {
            public static ProcessComparer Instance = new ProcessComparer();
            public bool Equals(Process x, Process y) { return x.Id == y.Id; }
            public int GetHashCode(Process obj) { return obj.Id; }
        }

        static void Main(string[] args)
        {
            IEnumerable<Process> collected = GetNewProcesses(TimeSpan.FromMinutes(1));
            Console.WriteLine(string.Join(Environment.NewLine, collected.Select(p => string.Join(" ", p.ProcessName, p.StartInfo.FileName))));
        }

        static IEnumerable<Process> GetNewProcesses(TimeSpan interval)
        {
            DateTime startTime = DateTime.Now;
            IEnumerable<Process> initial = Process.GetProcesses();
            List<Process> collected = new List<Process>();

            while (DateTime.Now - startTime < interval)
            {
                collected.AddRange(Process.GetProcesses().Except(initial, ProcessComparer.Instance));
            }
            return collected;
        }
    }
}
