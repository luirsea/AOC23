
using System.ComponentModel;
using System.Runtime.InteropServices;
using dotnet;

public class Day03{

    public static Dictionary<(int, int), List<int>> gears = new Dictionary<(int, int), List<int>>();

    public static (string flag1, string flag2) GetFlag(string[] lines){

        var partNumbers = new List<int>();

        partNumbers = ParseLine(null, lines[0], lines[1], 0);

        Program.DebugLine(string.Join(", ", partNumbers));
        for (int i = 1; i < lines.Length - 1; i++){
            var newPartNumbers = ParseLine(lines[i-1], lines[i], lines[i+1], i);

            partNumbers = partNumbers.Concat(newPartNumbers).ToList();
        }
        var lastNumbers = ParseLine(lines[lines.Length - 2], lines[lines.Length - 1], null, lines.Length - 1);

        partNumbers = partNumbers.Concat(lastNumbers).ToList();

        int gearRatioSum = 0;
        foreach (var gear in gears){
            var list = gear.Value;
            if (list.Count == 2){
                Program.DebugLine($"Gear Ratio: {list[0]}-{list[1]}");
                gearRatioSum += list[0] * list[1];
            }
        }

        return (partNumbers.Sum().ToString(), gearRatioSum.ToString());
    }
    
    public static List<int> ParseLine(string last, string line, string next, int curIndex){
        var partNumbers = new List<int>();

        int currentNumber = 0;
        bool isPartNumber = false;
        var adjStars = new HashSet<(int,int)>();
        for( int i = 0; i < line.Length; i++){

            if (int.TryParse(line[i].ToString(), out var digit)){
                currentNumber = (currentNumber * 10) + digit; // Shift digits one to right

                if (!isPartNumber){ // Only check for adjacent symbols if not already found
                    // Check all adjacent values for symbols 
                    if (i > 0){
                        if (isSymbol(line[i-1]) || 
                            (last != null && isSymbol(last[i-1])) || 
                            (next != null && isSymbol(next[i-1]))){
                                isPartNumber = true;
                            }
                    }

                    if ((last != null && isSymbol(last[i])) ||
                        (next != null && isSymbol(next[i]))){
                            isPartNumber = true;
                        }

                    if (i + 1 < line.Length){
                        if (isSymbol(line[i+1]) || 
                            (last != null && isSymbol(last[i+1])) || 
                            (next != null && isSymbol(next[i+1]))){
                            isPartNumber = true;
                            }
                    }
                }


                if (i > 0){
                    if (line[i-1] == '*')
                        adjStars.Add((i-1,0)); 
                    if (last != null && last[i-1] == '*')
                        adjStars.Add((i-1,-1)); 
                    if (next != null && next[i-1] == '*'){
                        adjStars.Add((i-1,1)); 
                        }
                }

                if (last != null && last[i] == '*')
                    adjStars.Add((i,-1));
                if (next != null && next[i] == '*')
                    adjStars.Add((i,1));


                if (i + 1 < line.Length){
                    if (line[i+1] == '*')
                        adjStars.Add((i+1,0)); 
                    if (last != null && last[i+1] == '*')
                        adjStars.Add((i+1,-1)); 
                    if (next != null && next[i+1] == '*'){
                        adjStars.Add((i+1,1)); 
                        }
                }
            } else {
                if (isPartNumber){
                    // End of a part number
                    Program.DebugLine($"Add: {currentNumber}");
                    partNumbers.Add(currentNumber);
                    isPartNumber = false;
                }
                foreach (var star in adjStars){
                    var absStar = (star.Item1, star.Item2 + curIndex);
                    if (!gears.ContainsKey(absStar)){
                        gears.Add(absStar, new List<int>(){currentNumber});
                    } else {
                        gears[absStar].Add(currentNumber);
                    }
                }

                adjStars.Clear();

                currentNumber = 0;
            }
        }

        if (isPartNumber){
            // End of a part number
            partNumbers.Add(currentNumber);
            isPartNumber = false;
            foreach (var star in adjStars){
                    var absStar = (star.Item1, star.Item2 + curIndex);
                    if (!gears.ContainsKey(absStar)){
                        gears.Add(absStar, new List<int>(){currentNumber});
                    } else {
                        gears[absStar].Add(currentNumber);
                    }
                }
        }

        return partNumbers;
    }

    public static bool isSymbol(char c){
        return c != '.' && !Char.IsDigit(c);
    }
}