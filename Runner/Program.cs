using System;
using System.Diagnostics;
using System.Linq;

namespace Runner {
    class Program {
        static void Main(string[] args) {
            if (!args.Any()) return;
            try {
                Process.Start(args[0]);
            } catch {
                Console.WriteLine("Error occured");
            }
        }
    }
}
