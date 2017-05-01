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
            IEnumerable<Process> initial = Process.GetProcesses();
            List<Process> collected = new List<Process>();

            DateTime startTime = DateTime.Now;
            TimeSpan interval = TimeSpan.FromMinutes(0.2);

            while (DateTime.Now - startTime < interval)
            {
                collected.AddRange(Process.GetProcesses().Except(initial, ProcessComparer.Instance));
            }

            Console.WriteLine(string.Join(Environment.NewLine, collected.Select(p => string.Join(" ", p.ProcessName, p.StartInfo.FileName))));
        }
    }
}
