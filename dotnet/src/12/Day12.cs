
using dotnet;

public class Day12{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        long totalSolves = 0;
        long totalUnfoldedSolves = 0;

        foreach (var line in lines){
            var split = line.Split(' ',2);

            var row = split[0].ToCharArray();
            var unfoldedRow = Enumerable.Repeat(split[0] + '?', 5).SelectMany(x => x).SkipLast(1).ToArray();

            var groups = split[1].Split(',').Select(long.Parse).ToList();
            var unfoldedGroups = Enumerable.Repeat(groups, 5).SelectMany(x => x).ToList();

            var lineCount = countSolves(row, groups);
            Program.DebugLine($"Line cnt: {lineCount}");
            totalSolves += lineCount;

            /*
            groups.Add(groups.First());
            var splitRow = split[0].Split('.', 2);
            if (splitRow.Length < 2){
                Program.DebugLine($"****No Split: for {string.Join("",row)}");
            }else
            {
                var rearrangedRow = (splitRow[1] + '?' + splitRow[0]).ToCharArray();
                var extraCount = countSolves(rearrangedRow, groups);
                if (extraCount > 0)
                    Program.DebugLine($"****Extra Line cnt: {extraCount} for {string.Join("",row)} - split {string.Join("", rearrangedRow)}");

                groups.Add(groups.Skip(1).First());
                var extraExtraCount = countSolves(rearrangedRow, groups);
                if (extraExtraCount > 0)
                    Program.DebugLine($"****Extra****Extra Line cnt: {extraExtraCount} for {string.Join("",row)} - split {string.Join("", rearrangedRow)}");

            }*/
            
            var unfoldedlineCount = countSolves(unfoldedRow, unfoldedGroups);
            Program.DebugLine($"Unfolded Line cnt: {unfoldedlineCount}");
            totalUnfoldedSolves += unfoldedlineCount;
            
        }


        return (totalSolves.ToString(), totalUnfoldedSolves.ToString());
    }


    public static long countSolves(char[] row, List<long> groups){

        var currentGroupCnt = 0;
        var currentGroupIndex = 0;
        for (long i = 0; i < row.Length; i++){
            if (row[i] == '.'){
                if (currentGroupCnt > 0){
                    if (currentGroupCnt != groups[currentGroupIndex]){
                        // Program.DebugLine($"{i}: {currentGroupCnt} != {groups[currentGroupIndex]} for {string.Join(' ',row)}");
                        return 0; // Invalid!
                    }
                    currentGroupCnt = 0;
                    currentGroupIndex++;
                }
            }
            else if (row[i] == '#')
            {
                currentGroupCnt++;

                if (currentGroupIndex >= groups.Count() || currentGroupCnt > groups[currentGroupIndex]){
                    // Program.DebugLine($"{i}: {currentGroupCnt} > {groups[currentGroupIndex]} for {string.Join(' ',row)}");
                    return 0; // Invalid!
                }
            }
            else // row[i] == #
            {
                row[i] = '.';
                //Program.DebugLine($"Trying: {string.Join(' ',row)}");
                long solvesCnt = countSolves(row, groups);
                //Program.DebugLine($"For:    {string.Join(' ',row)} got {solvesCnt}");

                row[i] = '#';
                //Program.DebugLine($"Trying: {string.Join(' ',row)}");
                long solvesCntB = countSolves(row, groups);
                //Program.DebugLine($"For:    {string.Join(' ',row)} got {solvesCntB}");

                solvesCnt += solvesCntB;

                row[i] = '?'; // reset for other branch of recursion

                return solvesCnt;
            }

        }

        if (row[row.Length - 1] == '#'){
            if (currentGroupCnt > 0){
                if (currentGroupCnt != groups[currentGroupIndex]){
                    // Program.DebugLine($"{i}: {currentGroupCnt} != {groups[currentGroupIndex]} for {string.Join(' ',row)}");
                    return 0; // Invalid!
                }
                currentGroupCnt = 0;
                currentGroupIndex++;
            }
        }

        if (currentGroupIndex == groups.Count()){
            //Program.DebugLine($"+1 for {string.Join(' ',row)}");
            return 1;
        } else {
            return 0;
        }
    }
}