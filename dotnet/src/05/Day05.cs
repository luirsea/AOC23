
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using dotnet;
using Microsoft.VisualBasic;

public class Day05{

    public static (string flag1, string flag2) GetFlag(string[] lines){
        var seeds = lines.First().Split(":", 2)[1].Trim().Split(' ').Select(long.Parse).ToArray();

        Program.DebugLine(seeds.First().ToString());

        // Parse maps
        var maps = new List<FullMap>();
        var translations = new List<Translation>();
        foreach (var line in lines.Skip(1)){
            if (string.IsNullOrWhiteSpace(line)){
                maps.Add(new FullMap(translations));
                translations = new List<Translation>();


            } else if (!char.IsDigit(line[0])){
                // map name line
            } else {
                var vals = line.Split(' ', 3).Select(long.Parse).ToArray();

                translations.Add(new Translation{
                    range = new Range{
                        from = vals[1],
                        to = vals[1] + vals[2],
                    },
                    offset = vals[0] - vals[1]
                });
            }

        }
        maps.Add(new FullMap(translations));

        // Find seeds
        var minNumber = long.MaxValue;

        foreach (var seed in seeds){
            var number = seed;
            foreach (var map in maps){
                number = map.Offset(number);
            }

            Program.DebugLine($"{seed} => {number}");
            if (number < minNumber){
                minNumber = number;
            }
        }

        //Seed ranges
        var seedRanges = new List<Range>();
        for (var i = 0; i < seeds.Count() - 1; i+=2 ){
            seedRanges.Add(new Range{
                from = seeds[0],
                to = seeds[0] + seeds[1],
            });
        }

        Program.DebugLine($"\n\nSeed ranges: {seedRanges.Count}");
        var minRangedNumber = long.MaxValue;
        
        
        foreach (var range in seedRanges){
            IEnumerable<Range> splitRanges = new List<Range>(){range};       
            foreach(var map in maps){
                Program.DebugLine($"splitRanges: {splitRanges.Count()}");
                IEnumerable<Range> newSplitRanges = new List<Range>();
                foreach(var split in splitRanges){
                    var splitOffsetRanges = map.OffsetRange(split);
                    newSplitRanges = newSplitRanges.Concat(splitOffsetRanges);
                }

                splitRanges = newSplitRanges;
            }

            var minNum = splitRanges.MinBy(range => range.from).from;
            if (minRangedNumber > minNum){
                minRangedNumber = minNum;
            }
        }

        return (minNumber.ToString(), minRangedNumber.ToString());
    }

    public class FullMap{
        private (long value, long offset)[] _map;
        public FullMap(IEnumerable<Translation> translations){
            var sorted = translations.OrderBy(translation => translation.range.from);

            long prevTo = 0;
            var map = new List<(long value, long offset)>();
            foreach (var translation in sorted){
                if (prevTo < translation.range.from){
                    map.Add((prevTo, 0));
                }
                map.Add((translation.range.from, translation.offset));
                prevTo = translation.range.to;
            }

            if (prevTo < long.MaxValue){
                map.Add((prevTo, 0));
            }

            map.Add((long.MaxValue, 0));

            _map = map.ToArray();
        }

        public long Offset(long value){
            for(int i = 0; i < _map.Length - 1; i++){
                if (value >= _map[i].value && value < _map[i+1].value){
                    return value + _map[i].offset;
                }
            }

            return value; // Will always be found above
        }

        public IEnumerable<Range> OffsetRange(Range inputRange){
            var splitOffsetRanges = new List<Range>();

            bool insideInput = false;
            for(int i = 0; i < _map.Length - 1; i++){
                var left = _map[i].value;
                var right = _map[i+1].value;
                var offset = _map[i].offset;
                if (!insideInput) // Left of inputRange
                {
                    if (inputRange.from >= left && inputRange.from < right ){
                        insideInput = true;
                        splitOffsetRanges.Add(new Range{
                            from = inputRange.from + offset,
                            to = Math.Min(inputRange.to, _map[i+1].value) + offset,
                        });
                    }
                } else {
                    if (inputRange.to >= left){ // inside inputRange
                        splitOffsetRanges.Add(new Range{
                            from = left + offset,
                            to = Math.Min(inputRange.to, right) + offset,
                        });

                        if (inputRange.to <= right){
                            break; // Found end of input range
                        }
                    }
                }
            }

            return splitOffsetRanges; // Will always be found above
        }
    }

    public record Translation{
        public Range range;
        public long offset;
    }

    public record Range{
        public long from;
        public long to; // not inclusive 
    }
}