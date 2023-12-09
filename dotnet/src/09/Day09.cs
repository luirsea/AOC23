
public class Day09{

    public static (string flag1, string flag2) GetFlag(string[] lines){

        int nextValueSum = 0;
        int firstValueSum = 0;

        foreach (var line in lines){
            var sequence = line.Split(' ').Select(int.Parse).ToArray();

            var (firstVal, nextVal) = nextValues(sequence);

            nextValueSum += nextVal;
            firstValueSum += firstVal;
        }

        return (nextValueSum.ToString(), firstValueSum.ToString());
    }

    public static (int before, int after) nextValues(int[] sequence){
        var finalReduce = false;
        var curSequence = sequence;

        var rhs = new List<int>();
        var lhs = new List<int>();

        while(!finalReduce){
            rhs.Add(curSequence[curSequence.Length - 1]);
            lhs.Add(curSequence[0]);
            (curSequence, finalReduce) = reduce(curSequence);
        }
        
        var before = 0;
        for (int i = 0; i < lhs.Count; i++){
            if (i % 2 == 0){
                before += lhs[i];
            } else {
                before -= lhs[i];
            }
        }

        return (before, rhs.Sum());
    }

    public static (int[], bool) reduce(int[] sequence){
        var reduced = new int[sequence.Length - 1];
        var finalReduce = true;

        for (int i = 0; i < reduced.Length; i++){
            var diff = sequence[i+1] - sequence[i];

            if (finalReduce && diff != 0)
                finalReduce = false;

            reduced[i] = diff;
        }

        return (reduced, finalReduce);
    }
}