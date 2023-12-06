
using dotnet;

public class Day06{

    public static (string flag1, string flag2) GetFlag(string input){

        var split = input.Split('\n', 2);
        var times = split[0].Split(":", 2)[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToArray();
        var distances = split[1].Split(":", 2)[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToArray();

        Program.DebugLine(times.Length.ToString());
        Program.DebugLine(distances.Length.ToString());

        var bigTime = long.Parse(split[0].Split(":", 2)[1].Replace(" ", ""));
        var bigDistance = long.Parse(split[1].Split(":", 2)[1].Replace(" ", ""));

        Program.DebugLine($"{bigTime} - {bigDistance}");

        long solnProd = 0;

        for(long i = 0; i < times.Length; i++){
            long b = times[i];
            long c = -distances[i];
            long solnRange = calcNumRaceSoln(times[i], distances[i]);

            if (solnProd == 0){
                solnProd = solnRange;
            } else {
                solnProd *= solnRange;
            }
        }

        long bigSolnRange = calcNumRaceSoln(bigTime, bigDistance);

        return (solnProd.ToString(), bigSolnRange.ToString());
    }

    //(-b±√(b²-4ac))/(2a)
    // a = -1
    // b = times
    // c = -distance
    // Solves Program as a quadratic. 
    // Answer is the num of integers between min winning time holding button and max winning time 
    public static long calcNumRaceSoln(long time, long distance){
            var b = time;
            var c = -distance;

            long sq = (b*b)+(4*c);

            if (sq < 0){
                return 0; // No winning times, input should not allow
            } 

            double sqrt = Math.Sqrt(sq);
            Program.DebugLine($"b: {b}, c: {c}, sqrt:{sqrt}");

            double lowersoln = (-b+sqrt)/(-2);
            double uppersoln = (-b-sqrt)/(-2);
            Program.DebugLine($"Lower: {lowersoln} - Upper: {uppersoln}");

            // number of integers between lower and upper solution, could probably be simplified
            long solnRange = Math.Abs(((long)Math.Ceiling(uppersoln)-1) - ((long)lowersoln + 1)) + 1; 
            Program.DebugLine(solnRange.ToString());

            return solnRange;
            
    }
}