using dotnet;


public class Day01{

    public static readonly List<(int value, string term)> SEARCH_TERMS = new List<(int, string)>()
        {(1, "one"),(2, "two"), (3, "three"), (4, "four"), (5, "five"), (6, "six"), (7, "seven"), (8, "eight"), (9, "nine")};
    public static (string flag1, string flag2) GetFlag(string[] lines){
        
        var totalResult = 0;
        var totalWordsResult = 0;

        foreach (var line in lines){

            int? first = null;
            int? last = null;

            // Same as first & last, but includes spelt digits
            int? firstWords = null;
            int? lastWords = null;
            
            int[] matchOffsets = new int[SEARCH_TERMS.Count()];

            Program.DebugLine(line);
            foreach (var c in line){
                
                // CASE: char is a digit
                if (int.TryParse(c.ToString(), out var digit)){
                    first = first ?? digit;
                    last = digit;

                    firstWords = firstWords ?? digit;
                    lastWords = digit;

                    Program.DebugLine($"Match: {digit}");

                    // Reset all searches as none contain digits
                    matchOffsets = new int[SEARCH_TERMS.Count()];
                } 
                else
                {
                    for (int i = 0; i < SEARCH_TERMS.Count(); i++){
                        var term = SEARCH_TERMS[i].term;
                        
                        if (term[matchOffsets[i]] != c){
                            matchOffsets[i] = 0;
                        }


                        if (term[matchOffsets[i]] == c) {
                            matchOffsets[i]++;

                            // Case: full word match
                            if (matchOffsets[i] >= term.Length){
                                Program.DebugLine($"Match!: {term}");
                                matchOffsets[i] = 0;
                                firstWords = firstWords ?? SEARCH_TERMS[i].value;
                                lastWords = SEARCH_TERMS[i].value;
                            } else {
                                Program.DebugLine($"Match {term} upto: {term.Substring(0, matchOffsets[i])}");
                            }
                        }
                    }
                }
            }

            if (firstWords.HasValue && lastWords.HasValue)
                totalWordsResult += (10 * firstWords.Value) + lastWords.Value;
            
            Program.DebugLine($"total (words): {firstWords}{lastWords}");


            if (first.HasValue && last.HasValue)
                totalResult += (10 * first.Value) + last.Value;
            
        }
        return (totalResult.ToString(), totalWordsResult.ToString());
    }
}