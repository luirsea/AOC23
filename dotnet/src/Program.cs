using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;

namespace dotnet;

class Program
{
    public static bool printDebug = false;
    static void Main(string[] args)
    {
        Console.WriteLine("Get them stars!");

        if (args.Count() < 1 || !int.TryParse(args[0], out int day))
        {
            Console.WriteLine("Please suply day number in application args.");
            return;
        }

        if (args.Count() > 1 && args[1] == "debug")
            printDebug = true;

        string input = "";

        if (args.Count() > 2){
            input = args[2];
        }
        else 
        {
            string inputFile = $"../../inputs/{day}.txt";
            try
            {
                using (var sr = new StreamReader(inputFile))
                {
                    input = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed reading input ({inputFile})\n {e}");
                return;
            }
        }

        var start = DateTime.Now;
        var flag = day switch {
            2201 => Day2203.GetFlag(input),
            1 => Day01.GetFlag(input.Split('\n')),
            2 => Day02.GetFlag(input.Split('\n')),
            3 => Day03.GetFlag(input.Split('\n')),
            4 => Day04.GetFlag(input.Split('\n')),
            5 => Day05.GetFlag(input.Split('\n')),
            6 => Day06.GetFlag(input),
            8 => Day08.GetFlag(input.Split('\n')),
            9 => Day09.GetFlag(input.Split('\n')),
            _ => ("No flags yet :(", null),
        };
        var end = DateTime.Now;

        Console.WriteLine("\n\n");

        if (flag.Item1 != null){
            Console.WriteLine($"Flag found!\t\t{flag.Item1}");
        }

        if (flag.Item2 != null){
            Console.WriteLine($"Flag 2 found as well!\t{flag.Item2}");
        }

        Console.WriteLine($"Time taken: {(end-start).TotalMilliseconds} ms");
    }

    public static void DebugLine(string msg){
        if (printDebug)
            Console.WriteLine(msg);
    }
    public static void Debug(string msg){
        if (printDebug)
            Console.Write(msg);
    }
}
