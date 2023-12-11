
using System.Security.Cryptography.X509Certificates;
using dotnet;

public class Day07{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        var handRanks = new List<(int bid, long rank, string hand)>();

        for (int i = 0; i < lines.Length; i++){
            var split = lines[i].Split(' ', 2);
            var hand = split[0];
            var bid = int.Parse(split[1]);

            var numCards = new long[NUMCARDS];

            long handValue = 0;

            foreach (char v in hand){
                var val = value(v);
                numCards[val]++;

            }

            var sortedHand = numCards.Skip(1).OrderDescending(); // split out Jockers
            var numJs = numCards[0];

            Program.DebugLine(hand);

            if (sortedHand.First() + numJs == 5){ // Five of a kind
                handValue = 6;
                Program.DebugLine("5");
            } else if (sortedHand.First() + numJs == 4){ // Four of a kind
                handValue = 5;
                Program.DebugLine("4");
            } else if (sortedHand.First() + numJs == 3 && sortedHand.Skip(1).First() == 2 ||
                sortedHand.First() == 3 && sortedHand.Skip(1).First() + numJs == 2){ //Full House
                handValue = 4;
                Program.DebugLine("3/2");
            } else if (sortedHand.First() + numJs == 3){
                handValue = 3;
                Program.DebugLine("3");
            } else if (sortedHand.First() == 2 && sortedHand.Skip(1).First() + numJs == 2){ //2 pair
                handValue = 2;
                Program.DebugLine("2/2");
            } else if (sortedHand.First() + numJs == 2){ // One Pair
                handValue = 1;
                Program.DebugLine("2");
            }

            foreach (char c in hand){
                handValue = (handValue * NUMCARDS) + value(c); // left shift current answer add new digit
            }

            handRanks.Add((bid, handValue, hand));
        }

        var sortedHands = handRanks.OrderBy(hand => hand.rank);
        int j = 1;
        long total = 0;

        foreach(var hand in sortedHands){
            Program.DebugLine($"{j} - {hand.hand} - {hand.rank}");
            total += hand.bid * j;
            j++;
        }
        

        return (total.ToString(), null);   
    }

    public static long value (char c) => c switch{
        'A' => 12, 
        'K' => 11, 
        'Q' => 10, 
        'T' => 9,
        '9' => 8, 
        '8' => 7, 
        '7' => 6,
        '6' => 5,
        '5' => 4, 
        '4' => 3,
        '3' => 2,
        '2' => 1,
        'J' => 0, 
    };

    public const long NUMCARDS = 13;
}