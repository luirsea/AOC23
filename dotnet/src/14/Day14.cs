
public class Day14{

    // public static char[] field._field;
    // public static int field.width;
    // public static int field.height;
    public static (string flag1, string flag2) GetFlag(string input){

        var lines = input.Split('\n');

        var fast = new field{
            _field = lines.SelectMany(line => line.ToCharArray()).ToArray(),
            height = lines.Length,
            width = lines[0].Length,
        } ;

         var slow = new field{
            _field = lines.SelectMany(line => line.ToCharArray()).ToArray(),
            height = lines.Length,
            width = lines[0].Length,
        } ;
        // char[][] result = new char[field.Length][];
        // for (int i = 0; i < result.Length; i++){
        //     result[i] = new char[field[i].Length];
        // }
        
        int i = 0;
        spin(fast);
        int first = 0;
        field loopStart = new field();

        int interval = 0;

        while(interval == 0){

            spin(fast);
            spin(fast);
            spin(slow);
            i++;
            if (fieldEqual(fast, slow)){
                Console.WriteLine(i);
                if (first == 0){
                    first = i;
                    var a = new char[slow._field.Length];
                    Array.Copy(slow._field, a, slow._field.Length);
                    loopStart = new field(){
                        _field = a,
                        width = slow.width,
                        height = slow.height,
                    };
                }
                else
                    interval = i - first;
            }

        }

        Console.WriteLine($"Loop! {first} - {interval}");

        for (int j = 0; j < (1000000000 - first) % interval; j++){
            spin(loopStart);
        }


        // for(int i = 0; i < 3; i++){
        //     tiltNorth(fast);
        //     //Console.WriteLine("North");
        //     //drawField();
        //     titlWest(fast);
        //     //Console.WriteLine("West");
        //     //drawField();
        //     titlSouth(fast);
        //     //Console.WriteLine("South");
        //     //drawField();
        //     titlEast(fast);
        //     //Console.WriteLine("East");
        //     drawField(fast);

        //     if (i % 10000000 == 0){
        //         Console.WriteLine("100000");
        //     }
        // }

        var totalLoad = 0; 
        for (int y = 0; y < fast.height; y++){
            int load = fast.height - y;
            for (int x = 0; x < fast.width; x++){
                if (fast._field[p(x,y, fast.width)] == 'O'){
                    //Console.WriteLine(load);
                    totalLoad += load;
                }
            }
        }

        var bigTotalLoad = 0; 
        for (int y = 0; y < loopStart.height; y++){
            int load = loopStart.height - y;
            for (int x = 0; x < loopStart.width; x++){
                if (loopStart._field[p(x,y, loopStart.width)] == 'O'){
                    //Console.WriteLine(load);
                    bigTotalLoad += load;
                }
            }
        }

        return (totalLoad.ToString(), bigTotalLoad.ToString());
    }

    public static void spin(field field){
        tiltNorth(field);
        titlWest(field);
        titlSouth(field);
        titlEast(field);
    }

    public static void drawField(field field){
        Console.Write('\n');
        for (int y = 0; y < field.height; y++){
            for (int x = 0; x < field.width; x++){
                Console.Write(field._field[p(x,y, field.width)]);
            }
            Console.Write('\n');
        }
    }

    public static void tiltNorth(field field){
        for (int x = 0; x < field.width; x++){
            int curStack = 0;
            for (int y = field.height - 1; y >= 0; y--){
                if (field._field[p(x,y, field.width)] == 'O'){
                    curStack++;
                    field._field[p(x,y, field.width)] = '.';
                    // Console.WriteLine($"Found at ({x}, {y})");
                }
                else if (field._field[p(x,y, field.width)] == '#'){
                    field._field[p(x,y, field.width)] = '#';
                    for (int i = 1; i <= curStack; i++){
                        // var load = 1 + field.Length - (y + i);
                        // Console.WriteLine($"({x}, {y + i})");
                        // totalLoad += load;
                        field._field[p(x, y+i, field.width)] = 'O';
                    }

                    curStack = 0;
                } else {
                    field._field[p(x,y, field.width)] = '.';
                }
            }
            for (int i = 0; i < curStack; i++){
                // var load = 1 + field.Length - i;
                // Console.WriteLine($"({x}, {i}) - {load}");
                // totalLoad += load;
                field._field[p(x,i, field.width)] = 'O';
            }
        }
    }

    public static void titlSouth(field field){
        for (int x = 0; x < field.width; x++){
            int curStack = 0;
            for (int y = 0; y < field.height; y++){
                if (field._field[p(x,y, field.width)] == 'O'){
                    curStack++;
                    field._field[p(x,y, field.width)] = '.';
                    // Console.WriteLine($"Found at ({x}, {y})");
                }
                else if (field._field[p(x,y, field.width)] == '#'){
                    field._field[p(x,y, field.width)] = '#';
                    for (int i = 1; i <= curStack; i++){
                        // var load = 1 + field.Length - (y + i);
                        // Console.WriteLine($"({x}, {y + i})");
                        // totalLoad += load;
                        field._field[p(x, y-i, field.width)] = 'O';
                    }

                    curStack = 0;
                } else {
                    field._field[p(x,y, field.width)] = '.';
                }
            }
            for (int i = 0; i < curStack; i++){
                // var load = 1 + field.Length - i;
                // Console.WriteLine($"({x}, {i}) - {load}");
                // totalLoad += load;
                field._field[p(x, field.height - 1 - i, field.width)] = 'O';
            }
        }
    }

    public static void titlEast(field field){
        for (int y = 0; y < field.height; y++){
            int curStack = 0;
            for (int x = 0; x < field.width; x++){
                if (field._field[p(x,y, field.width)] == 'O'){
                    curStack++;
                    field._field[p(x,y, field.width)] = '.';
                    // Console.WriteLine($"Found at ({x}, {y})");
                }
                else if (field._field[p(x,y, field.width)] == '#'){
                    field._field[p(x,y, field.width)] = '#';
                    for (int i = 1; i <= curStack; i++){
                        // var load = 1 + field.Length - (y + i);
                        // Console.WriteLine($"({x}, {y + i})");
                        // totalLoad += load;
                        field._field[p(x-i,y, field.width)] = 'O';
                    }

                    curStack = 0;
                } else {
                    field._field[p(x,y, field.width)] = '.';
                }
            }
            for (int i = 0; i < curStack; i++){
                // var load = 1 + field.Length - i;
                // Console.WriteLine($"({x}, {i}) - {load}");
                // totalLoad += load;
                field._field[p(field.width- 1 - i,y, field.width)] = 'O';
            }
        }
    }

    public static void titlWest(field field){
        for (int y = 0; y < field.height; y++){
            int curStack = 0;
            for (int x = field.width - 1; x >= 0; x--){
                if (field._field[p(x,y, field.width)] == 'O'){
                    curStack++;
                    field._field[p(x,y, field.width)] = '.';
                    // Console.WriteLine($"Found at ({x}, {y})");
                }
                else if (field._field[p(x,y, field.width)] == '#'){
                    field._field[p(x,y, field.width)] = '#';
                    for (int i = 1; i <= curStack; i++){
                        // var load = 1 + field.Length - (y + i);
                        // Console.WriteLine($"({x}, {y + i})");
                        // totalLoad += load;
                        field._field[p(x+i,y, field.width)] = 'O';
                    }

                    curStack = 0;
                } else {
                    field._field[p(x,y, field.width)] = '.';
                }
            }
            for (int i = 0; i < curStack; i++){
                // var load = 1 + field.Length - i;
                // Console.WriteLine($"({x}, {i}) - {load}");
                // totalLoad += load;
                field._field[p(i,y, field.width)] = 'O';
            }
        }
    }

    public record field{
        public char[] _field;
        public int width;
        public int height;
    }

    public static int p(int x, int y, int width){
        return y * width + x;
    }

    public static bool fieldEqual(field a, field b){
        for(int i = 0; i < a._field.Length; i++){
            if (a._field[i] != b._field[i])
                return false;
        }

        return true;
    }
}