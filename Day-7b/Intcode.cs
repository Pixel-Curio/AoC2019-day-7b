using System;
using System.Collections.Generic;

namespace Day_7b
{
    class Intcode
    {
        private int _pointer;
        private readonly int[] _code;
        private List<int> _input = new List<int>();
        private int _lastOutput = 0;

        private const string AddCommand = "01";
        private const string MultiplyCommand = "02";
        private const string InputCommand = "03";
        private const string OutputCommand = "04";
        private const string JumpIfTrueCommand = "05";
        private const string JumpIfFalseCommand = "06";
        private const string LessThanCommand = "07";
        private const string EqualsCommand = "08";
        private const string ExitCommand = "99";

        public Intcode(int[] code) => _code = code;

        public (bool, int) Process() => Process(null);

        public (bool, int) Process(List<int> input)
        {
            if (input != null) _input.AddRange(input);

            while (true)
            {
                var command = ConsumeOpCode().ToString();
                var opCode = command.Length < 2 ? "0" + command : command[^2..];
                var parameters = command.Length > 2 ? command[..^2].PadLeft(4, '0') : "0000";

                if (opCode == AddCommand) Add(parameters);
                else if (opCode == MultiplyCommand) Multiply(parameters);
                else if (opCode == InputCommand) Input(parameters);
                else if (opCode == OutputCommand) 
                    return (false, Output(parameters));
                else if (opCode == JumpIfTrueCommand) JumpIfTrue(parameters);
                else if (opCode == JumpIfFalseCommand) JumpIfFalse(parameters);
                else if (opCode == LessThanCommand) LessThan(parameters);
                else if (opCode == EqualsCommand) Equals(parameters);
                else if (opCode == ExitCommand) 
                    return (true, _lastOutput);
            }
        }

        private void Add(string parameters)
        {
            int a = ConsumeOpCode();
            int b = ConsumeOpCode();
            int target = ConsumeOpCode();

            a = parameters[^1] == '0' ? _code.ExpandingGet(a) : a;
            b = parameters[^2] == '0' ? _code.ExpandingGet(b) : b;

            _code.ExpandingSet(target, a + b);
        }

        private void Multiply(string parameters)
        {
            int a = ConsumeOpCode();
            int b = ConsumeOpCode();
            int target = ConsumeOpCode();

            a = parameters[^1] == '0' ? _code.ExpandingGet(a) : a;
            b = parameters[^2] == '0' ? _code.ExpandingGet(b) : b;

            _code.ExpandingSet(target, a * b);
        }

        private void Input(string parameters)
        {
            int target = ConsumeOpCode();
            int input;

            if (_input.Count > 0)
            {
                input = _input[0];
                _input.RemoveAt(0);
            }
            else
            {
                Console.WriteLine($"Waiting for input at ({target}): ");
                input = Convert.ToInt32(Console.ReadLine());
            }

            _code.ExpandingSet(target, input);
        }

        private int Output(string parameters)
        {
            int target = ConsumeOpCode();

            int value = parameters[^1] == '0' ? _code.ExpandingGet(target) : target;
            _lastOutput = value;

            return value;
        }

        private void JumpIfTrue(string parameters)
        {
            int condition = ConsumeOpCode();
            int target = ConsumeOpCode();

            condition = parameters[^1] == '0' ? _code.ExpandingGet(condition) : condition;
            target = parameters[^2] == '0' ? _code.ExpandingGet(target) : target;

            if (condition != 0) _pointer = target;
        }

        private void JumpIfFalse(string parameters)
        {
            int condition = ConsumeOpCode();
            int target = ConsumeOpCode();

            condition = parameters[^1] == '0' ? _code.ExpandingGet(condition) : condition;
            target = parameters[^2] == '0' ? _code.ExpandingGet(target) : target;

            if (condition == 0) _pointer = target;
        }

        private void LessThan(string parameters)
        {
            int a = ConsumeOpCode();
            int b = ConsumeOpCode();
            int target = ConsumeOpCode();

            a = parameters[^1] == '0' ? _code.ExpandingGet(a) : a;
            b = parameters[^2] == '0' ? _code.ExpandingGet(b) : b;

            _code.ExpandingSet(target, a < b ? 1 : 0);
        }

        private void Equals(string parameters)
        {
            int a = ConsumeOpCode();
            int b = ConsumeOpCode();
            int target = ConsumeOpCode();

            a = parameters[^1] == '0' ? _code.ExpandingGet(a) : a;
            b = parameters[^2] == '0' ? _code.ExpandingGet(b) : b;

            _code.ExpandingSet(target, a == b ? 1 : 0);
        }

        public int GetValue(int index) => _code[index];

        public Intcode BufferInput(List<int> input)
        {
            _input.AddRange(input);
            return this;
        }

        public int GetLastOutput() => _lastOutput;

        private int ConsumeOpCode() => _pointer < _code.Length ? _code[_pointer++] : 99;
    }

    public static class ArrayExtensions
    {
        public static void ExpandingSet(this int[] source, int index, int value)
        {
            if (index > source.Length) Array.Resize(ref source, index + 1);
            source[index] = value;
        }
        public static int ExpandingGet(this int[] source, int index)
        {
            if (index > source.Length) Array.Resize(ref source, index + 1);
            return source[index];
        }
    }
}