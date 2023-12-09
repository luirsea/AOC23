
using dotnet;

public class Day08{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        var directions = lines[0];

        var map = new Dictionary<string, Node>();

        var positions = new List<string>();

        for(int i = 2; i < lines.Length; i++)
        {
            var split = lines[i].Split('=', 2);
            var name = split[0].Trim();

            var splts = split[1].Trim().Trim('(').Trim(')').Split(",");

            var left = splts[0].Trim();
            var right = splts[1].Trim();

            map.Add(name, new Node()
            {
                name = name,
                left = left,
                right = right,
            });

            if (name.EndsWith('A'))
            {
                positions.Add(name);
            }
        }

        var pos = "AAA";
        var steps = 0;

        while(pos != "ZZZ")
        {
            Program.DebugLine(pos);
            if (directions[steps %  directions.Length] == 'R')
            {
                pos = map[pos].right;
            } 
            else
            {
                pos = map[pos].left;
            }

            steps++;
        }

        var ghostPositions = positions.ToArray();

        Program.DebugLine($"GhostPositions: {ghostPositions.Length}");

        var ghostSteps = 0;
        while(ghostPositions.Any(x => !x.EndsWith("Z")))
        {
            for (var i = 0; i < ghostPositions.Length; i++)
            {
                if (directions[steps % directions.Length] == 'R')
                {
                    ghostPositions[i] = map[ghostPositions[i]].right;
                }
                else
                {
                    ghostPositions[i] = map[ghostPositions[i]].left;
                }
            }
            ghostSteps++;
        }


        return (steps.ToString(), ghostSteps.ToString());
    }

    public record Node()
    {
        public string name;
        public string left;
        public string right;
    }
}