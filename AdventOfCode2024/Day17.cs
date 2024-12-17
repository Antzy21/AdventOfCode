


using System.Text;

namespace AdventOfCode2024;

public class Day17 : IDaySolution
{
    public int? SolvePart1()
    {
        var computer = ParseInput();

        var output = computer.RunInstructions();
        Console.WriteLine($"{computer}\n------");
        Console.WriteLine($"{output}");

        return 0;
    }

    public int? SolvePart2()
    {
        return null;
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
    public int RegisterA { get; set; } = RegisterA;
    public int RegisterB { get; set; } = RegisterB;
    public int RegisterC { get; set; } = RegisterC;
    public List<int> Program { get; set; } = Program;

    public int ptr = 0;

    public List<int> output = [];

    public override string ToString()
    {
        return $"Register A: {RegisterA}\n" +
            $"Register B: {RegisterB}\n" +
            $"Register C: {RegisterC}\n"+
            $"\n" +
            $"Program: {string.Join(',', Program)}\n" +
            string.Join("", Enumerable.Repeat(' ', 9 + ptr*2)) + "^ ^";
    }

    internal string RunInstructions()
    {
        while (ptr < Program.Count)
        {
            RunInstruction();
            ptr += 2;
        }
        return string.Join(',', output);
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
                break;
            case 6:
                Bdv(operand);
                break;
            case 7:
                Cdv(operand);
                break;
        }
    }

    private int GetComboOp(int operand) {
        return operand switch
        {
            0 or 1 or 2 or 3 => operand,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => throw new Exception($"Bad Combo Operand: {operand}"),
        };
    }

    private void Adv(int operand) => RegisterA >>= GetComboOp(operand);

    private void Bxl(int operand) => RegisterB ^= operand;

    private void Bst(int operand) => RegisterB = GetComboOp(operand) % 8;

    private void Jnx(int operand) => ptr = RegisterA == 0 ? ptr : operand - 2;

    private void Bxc(int _) => RegisterB ^= RegisterC;

    private void Out(int operand) => output.Add(GetComboOp(operand) % 8);

    private void Bdv(int operand) => RegisterB = RegisterA >> operand;

    private void Cdv(int operand) => RegisterC = RegisterA >> operand;
}