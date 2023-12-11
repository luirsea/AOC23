
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using dotnet;

public class Day10{

    public static char[][] _field;
    public static bool[][] _loopPipe;
    public static (string flag1, string flag2) GetFlag(string input){
        
        _field = input.Split('\n').Select(line => line.ToCharArray()).ToArray();

        (int x, int y) start = (-1,-1);

        for (int y = 0; y < _field.Length; y++){
            for (int x = 0; x < _field[y].Length; x++){
                if (_field[y][x] == 'S'){
                    start = (x, y);
                }
            }

            if (start != (-1,-1)){
                break;
            }
        }

        (int x, int y) forwards = (-1,-1);
        (int x, int y) backwards = (-1,-1);

        if (traverse(start, (start.x + 1, start.y)) != (-1,-1)){
            forwards = (start.x + 1, start.y);
        }

        if (traverse(start, (start.x, start.y + 1)) != (-1,-1)){
            if (forwards == (-1,-1))
                forwards = (start.x, start.y + 1);
            else
                backwards = (start.x, start.y + 1);
        }

        if (traverse(start, (start.x - 1, start.y)) != (-1,-1)){
            if (forwards == (-1,-1))
                forwards = (start.x - 1, start.y);
            else
                backwards = (start.x - 1, start.y);
        }

        if (traverse(start, (start.x, start.y - 1)) != (-1,-1)){
            if (forwards == (-1,-1))
                forwards = (start.x, start.y - 1);
            else
                backwards = (start.x, start.y - 1);
        }

        var loopFound = false;
        var steps = 0;

        var lastForwards = start;
        var lastBackwards = start;

        _loopPipe = new bool[_field.Length][];
        for (int i = 0; i < _loopPipe.Length; i++)
        {
            _loopPipe[i] = new bool[_field[0].Length];
        }

        _loopPipe[start.y][start.x] = true;
        _loopPipe[forwards.y][forwards.x] = true;
        _loopPipe[backwards.y][backwards.x] = true;

        while (!loopFound)
        {
            steps++;

            var temp = forwards;
            forwards = traverse(lastForwards, forwards);
            lastForwards = temp;
            Program.DebugLine($"Step forwards: {lastForwards} -> {forwards} '{_field[lastForwards.y][lastForwards.x]}'");
            _loopPipe[forwards.y][forwards.x] = true;

            temp = backwards;
            backwards = traverse(lastBackwards, backwards);
            lastBackwards = temp;
            Program.DebugLine($"Step backwards: {lastBackwards} -> {backwards} '{_field[lastBackwards.y][lastBackwards.x]}'");
            _loopPipe[backwards.y][backwards.x] = true;

            if (lastBackwards == forwards || forwards == backwards){ //Loop found!
                loopFound = true;
            }
        }

        var area = new bool[_field.Length][];
        for (int i = 0; i < area.Length; i++)
        {
            area[i] = new bool[_field[0].Length];
        }

        var loopArea = 0;
        for (int y = 0; y < _loopPipe.Length; y++){
            bool insideLoop = false;
            int percent = 0;
            int tempArea = 0;
            Program.Debug("\n");
            for (int x = 0; x < _loopPipe[y].Length; x++){
                if (_loopPipe[y][x]) 
                {
                    Program.Debug($"\u001b[32m{_field[y][x]}\u001b[30m");
                    if (_field[y][x] == '|'){
                        percent = 100;
                    } else if (_field[y][x] == 'F' || _field[y][x] == 'J'){
                        percent += 50;
                    } else if (_field[y][x] == 'L' || _field[y][x] == '7'){
                        percent -= 50;
                    }

                    if (percent >= 100 || percent <= -100){
                        //Program.DebugLine($"Toggle {x},{y}");
                        insideLoop = !insideLoop;
                        loopArea += tempArea;
                        tempArea = 0;
                        percent = 0;
                    }
                    continue;
                }

                if (insideLoop){
                    //Program.DebugLine($"Loop Area {x},{y}");
                    tempArea++;
                    Program.Debug("\u001b[31m1\u001b[30m");
                }else {
                    Program.Debug("\u001b[34m0\u001b[30m");
                }
            }
        }

        return (steps.ToString(), loopArea.ToString());
    }

    public static (int x, int y) traverse((int x, int y) from, (int x, int y) thr){
        var to = (-1,-1);

        switch (_field[thr.y][thr.x]){
            case '|':
                if (from.x == thr.x && from.y == thr.y - 1){
                    to = (thr.x, thr.y + 1);
                }
                else if (from.x == thr.x && from.y == thr.y + 1){
                    to = (thr.x, thr.y - 1);
                }
                break;
            case '-':
                if (from.x == thr.x - 1 && from.y == thr.y){
                    to = (thr.x + 1, thr.y);
                }
                else if (from.x == thr.x + 1 && from.y == thr.y){
                    to = (thr.x - 1, thr.y);
                }
                break;
            case 'F':
                if (from.x == thr.x && from.y == thr.y + 1){
                    to = (thr.x + 1, thr.y);
                }
                else if (from.x == thr.x + 1 && from.y == thr.y){
                    to = (thr.x, thr.y + 1);
                }
                break;
            case 'L':
                if (from.x == thr.x && from.y == thr.y - 1){
                    to = (thr.x + 1, thr.y);
                }
                else if (from.x == thr.x + 1 && from.y == thr.y){
                    to = (thr.x, thr.y - 1);
                }
                break;
            case 'J':
                if (from.x == thr.x && from.y == thr.y - 1){
                    to = (thr.x - 1, thr.y);
                }
                else if (from.x == thr.x - 1 && from.y == thr.y){
                    to = (thr.x, thr.y - 1);
                }
                break;
            case '7':
                if (from.x == thr.x && from.y == thr.y + 1){
                    to = (thr.x - 1, thr.y);
                }
                else if (from.x == thr.x -1 && from.y == thr.y){
                    to = (thr.x, thr.y + 1);
                }
                break;
        }

        if (to == (-1,-1)){
            Program.DebugLine($"Bad Pipe! {from} -> {thr} '{_field[thr.y][thr.x]}'");
        }
        return to;
    }
}