
using dotnet;

public class Day04{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        int totalPoints = 0;
        int[] numWins = lines.Select(line => {
            var card = parseLine(line);
            return card.winners.Intersect(card.nums).Count();
        }).ToArray();

        totalPoints = numWins.Sum(numWins => (int)Math.Pow(2, numWins-1));

        int[] numCards = Enumerable.Repeat(1, numWins.Count()).ToArray();
        
        for(var i = 0; i < numCards.Length; i++){
            for (var j = 1; j <= numWins[i]; j++){
                numCards[i + j] += numCards[i];
            }
        }

        var totalCards = numCards.Sum();
        return (totalPoints.ToString(), totalCards.ToString());
    }

    public static Card parseLine(string line){
        var split = line.Split(':', 2);
        var cardNo = int.Parse(split[0].Substring(5));

        var numSplit = split[1].Split('|', 2);
        var winners = numSplit[0].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse);
        var nums = numSplit[1].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse);

        // Program.DebugLine($"{cardNo}: {winners.Count()} - {nums.Count()}");
        return new Card{
            cardNo = cardNo,
            winners = winners,
            nums = nums,
        };
    }


    public record Card{
        public int cardNo;
        public IEnumerable<int> winners;
        public IEnumerable<int> nums;
    }
}