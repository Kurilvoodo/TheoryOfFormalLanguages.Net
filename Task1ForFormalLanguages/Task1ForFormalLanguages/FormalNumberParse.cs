using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Task1ForFormalLanguages
{
    public class FormalNumberParse
    {
        public readonly string Value;
        public readonly string StartState = "q0"; //Constant meaning of Statment
        public readonly string FinishState = "q2";
        public string CurrentState { get; private set; }
        public List<(bool Status, int CountSymbols, string OutputInfo)> RunState { get; private set; }

        public FormalNumberParse(string input)
        {
            Value = input;
            CurrentState = StartState;
            RunState = new List<(bool Status, int CountSymbols, string OutputInfo)>();
        }

        //Shell method
        public void Max()
        {
            (bool Status, int CountNumber) currentIteration = Max(Value, 0); //C# 7.0 Cortege
            while (currentIteration.Status != false)
            {
                RunState.Add((currentIteration.Status, currentIteration.CountNumber, Value[currentIteration.CountNumber - 1].ToString()));
                currentIteration = Max(Value, currentIteration.CountNumber);
            }
        }

        private (bool Status, int CountNumber) Max(string input, int skip)
        {
            if (skip == input.Length)
            {
                return (false, skip);
            }

            if (CurrentState == "q0")
            {
                if (input[0] == '+' || input[0] == '-')
                {
                    CurrentState = "q1";
                    return (true, 1);
                }
                else if (char.IsDigit(input[0]))
                {
                    CurrentState = "q2";
                    return (true, 1);
                }
                return (false, 0);
            }
            else if (CurrentState == "q1")
            {
                if (char.IsDigit(input[1]))
                {
                    CurrentState = "q2";
                    return (true, 2);
                }
                return (false, 1);
            }
            else if (CurrentState == "q2")
            {
                if (char.IsDigit(input[skip]))
                {
                    return (true, ++skip);
                }
                return (false, skip);
            }

            throw new ArgumentOutOfRangeException("CurrentState", $"Incorrect current state {CurrentState}");
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
