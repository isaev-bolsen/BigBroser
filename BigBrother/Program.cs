using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BigBrother
{
    class Program
    {

        static void Main(string[] args)
        {
            IEnumerable<Process> initial = Process.GetProcesses();
            List<Process> collected = new List<Process>();

            DateTime startTime = DateTime.Now;
            TimeSpan interval = TimeSpan.FromMinutes(1);

            while (DateTime.Now - startTime < interval) collected.AddRange(Process.GetProcesses().Except(initial));

            Console.WriteLine(string.Join(Environment.NewLine, collected.Select(p => p.StartInfo.FileName)));
        }
    }
}
