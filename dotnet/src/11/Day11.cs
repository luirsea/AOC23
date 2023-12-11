
using dotnet;

public class Day11{

    public static (string flag1, string flag2) GetFlag(string input){

        char[][] sky = input.Split('\n').Select(line => line.ToCharArray()).ToArray();

        bool[] emptyCol = new bool[sky.Length];
        
        for (int x = 0; x < sky[0].Length; x++){
            bool empty = true;
            for (int y = 0; y < sky.Length; y++){
                if (sky[y][x] == '#'){
                    empty = false;
                    break;
                }
            }

            emptyCol[x] = empty;
        }

        var galaxies = new List<(int x, int y)>();
        var spreadGalaxies = new List<(long x, long y)>();
        
        int yExp = 0;
        long yBigExp = 0;
        for (int y = 0; y < sky.Length; y++){
            yExp++;
            yBigExp ++;
            
            int xExp = 0;
            long xBigExp = 0;
            bool empty = true;
            for (int x = 0; x < sky[y].Length; x++){
                xExp++;
                xBigExp ++;
                if (emptyCol[x]){
                    Program.DebugLine($"Expanding col {x}");
                    xExp++; // Expand a col
                    xBigExp += 1000000-1;
                }

                if (sky[y][x] == '#'){
                    empty = false;
                    galaxies.Add((xExp, yExp));
                    spreadGalaxies.Add((xBigExp, yBigExp));
                }
            }

            if (empty){
                Program.DebugLine($"Expanding row {y}");
                yExp++; // Expand a row
                yBigExp += 1000000-1;
            }
        }

        Program.DebugLine($"Galaxies: {galaxies.Count()}");

        var totalDistance = 0;
        for (int i = 0; i < galaxies.Count(); i++){
            for (int j = i+1; j < galaxies.Count(); j++){
                var manhattenDist = Math.Abs(galaxies[j].x - galaxies[i].x) + Math.Abs(galaxies[j].y - galaxies[i].y);

                totalDistance += manhattenDist;
            }
        }

        long totalSpreadDistance = 0;
        for (int i = 0; i < spreadGalaxies.Count(); i++){
            for (int j = i+1; j < spreadGalaxies.Count(); j++){
                var manhattenDist = Math.Abs(spreadGalaxies[j].x - spreadGalaxies[i].x) + Math.Abs(spreadGalaxies[j].y - spreadGalaxies[i].y);

                totalSpreadDistance += manhattenDist;
            }
        }

        return (totalDistance.ToString(), totalSpreadDistance.ToString());
    }
}