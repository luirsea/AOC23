
public class Day2203{

    public static (string flag1, string flag2) GetFlag(string input){

        var totalCalCount = new List<int>();

        int currentCalCount = 0;
        foreach(var line in input.Split('\n')){
            if (!string.IsNullOrWhiteSpace(line)){
                currentCalCount += int.Parse(line);
            }else{
                totalCalCount.Add(currentCalCount);
                currentCalCount = 0;
            }
        }

        var sort = totalCalCount.OrderDescending();

        var maxCals = sort.First().ToString();

        var max3Cals = sort.Take(3).Sum().ToString();


        return (maxCals, max3Cals);
    }
}