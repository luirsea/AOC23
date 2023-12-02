
public class Day02{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        int totalPossible = 0;
        int totalPowerSum = 0;

        const int MAX_RED = 12;
        const int MAX_GREEN = 13;
        const int MAX_BLUE = 14;

        foreach(var line in lines){
            var split = line.Split(':', 2);
            var gameID = int.Parse(split[0].Split(' ')[1]);
            var games = split[1].Split(';');

            bool impossible = false;

            var maxRed = 0;
            var maxGreen = 0;
            var maxBlue = 0;

            foreach(var game in games){
                var (redCount, greenCount, blueCount) = GetCounts(game);

                if (redCount > MAX_RED ||
                    greenCount > MAX_GREEN ||
                    blueCount > MAX_BLUE){
                        impossible = true;
                    }

                if (maxRed < redCount)
                    maxRed = redCount;
                if (maxGreen < greenCount)
                    maxGreen = greenCount;
                if (maxBlue < blueCount)
                    maxBlue = blueCount;
            }

            totalPowerSum += (maxRed * maxGreen * maxBlue);

            if (!impossible)
                totalPossible += gameID;
        }

        return (totalPossible.ToString(), totalPowerSum.ToString());
    }

    public static (int red, int green, int blue) GetCounts(string game){
        var red = 0;
        var blue = 0;
        var green = 0;

        foreach (var count in game.Split(',')){
            var split = count.Trim().Split(' ');
            var n = split[0];
            var colour = split[1];

            if (colour == "red"){
                red += int.Parse(n);
            } else if (colour == "green"){
                green += int.Parse(n);
            } else if (colour == "blue"){
                blue += int.Parse(n);
            }
        }

        return (red, green, blue);
    }
}