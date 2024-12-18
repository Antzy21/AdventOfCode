namespace AdventOfCode2024;

public class Day17 : IDaySolution
{
    public int? SolvePart1()
    {
        var computer = ParseInput();

        var output = computer.RunInstructions();
        Console.WriteLine($"Day 17 Part 1: {output}");

        return 0;
    }

    public int? SolvePart2()
    {
        var computer = ParseInput();

        var output = computer.RunInstructions();
        var expected = string.Join(',', computer.Program);

        long a = 46672426; // 46672429, 46672431 also work.

        while (output != expected)
        {
            a += 2<<26;
            computer.RegisterA = a;
            computer.RegisterB = 0;
            computer.RegisterC = 0;
            computer.ptr = 0;
            computer.output = [];
            output = computer.RunInstructions(true);
        }

        Console.WriteLine($"Day 17 Part 2: {a}");

        return (int)a;
    }

    private static Computer ParseInput()
    {
        var lines = File.ReadAllLines("inputs/day17.txt");
        var program = lines[^1][8..]
            .Split(',')
            .Select(int.Parse)
            .ToList();
        return new Computer(
            program,
            int.Parse(lines[0][12..]),
            int.Parse(lines[1][12..]),
            int.Parse(lines[2][12..])
        );
    }
}

internal class Computer(List<int> Program, int RegisterA, int RegisterB, int RegisterC)
{
    public long RegisterA { get; set; } = RegisterA;
    public long RegisterB { get; set; } = RegisterB;
    public long RegisterC { get; set; } = RegisterC;
    public List<int> Program { get; set; } = Program;

    public int ptr = 0;

    public List<int> output = [];
    private bool _halt = false;
    private bool _expectProgram = false;

    public string OutputString { get { return string.Join(',', output); } }

    public override string ToString()
    {
        return "---------------------\n" +
            $"|Register A: {RegisterA}\n" +
            $"|Register B: {RegisterB}\n" +
            $"|Register C: {RegisterC}\n" +
            $"|\n" +
            $"|Program: {string.Join(',', Program)}\n" +
            "|" + string.Join("", Enumerable.Repeat(' ', 9 + ptr * 2)) + "^ ^\n" +
            $"|'{OutputString}'\n" +
            "---------------------";
    }

    internal string RunInstructions(bool expectProgram = false)
    {
        _expectProgram = expectProgram;
        _halt = false;
        while (ptr < Program.Count && !_halt)
        {
            RunInstruction();
            ptr += 2;
        }
        return OutputString;
    }

    private void RunInstruction()
    {
        var opcode = Program[ptr];

        var operand = Program[ptr + 1];

        switch (opcode)
        {
            case 0:
                Adv(operand);
                break;
            case 1:
                Bxl(operand);
                break;
            case 2:
                Bst(operand);
                break;
            case 3:
                Jnx(operand);
                break;
            case 4:
                Bxc(operand);
                break;
            case 5:
                Out(operand);
                if (_expectProgram && output[^1] != Program[output.Count - 1])
                    _halt = true;
                break;
            case 6:
                Bdv(operand);
                break;
            case 7:
                Cdv(operand);
                break;
        }
    }

    private long GetComboOp(int operand)
    {
        return operand switch
        {
            0 or 1 or 2 or 3 => operand,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => throw new Exception($"Bad Combo Operand: {operand}"),
        };
    }

    private void Adv(int operand) => RegisterA = RegisterA >> (int)GetComboOp(operand);

    private void Bxl(int operand) => RegisterB ^= operand;

    private void Bst(int operand) => RegisterB = GetComboOp(operand) % 8;

    private void Jnx(int operand) => ptr = RegisterA == 0 ? ptr : operand - 2;

    private void Bxc(int _) => RegisterB ^= RegisterC;

    private void Out(int operand) => output.Add((int)(GetComboOp(operand) % 8));

    private void Bdv(int operand) => RegisterB = RegisterA >> (int)GetComboOp(operand);

    private void Cdv(int operand) => RegisterC = RegisterA >> (int)GetComboOp(operand);
}