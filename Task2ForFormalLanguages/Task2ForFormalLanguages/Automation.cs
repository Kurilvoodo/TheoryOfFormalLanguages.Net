using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task2ForFormalLanguages
{
    public class Automation
    {
        private string inputInformation;

        private string startState;
        private string finishState;

        private string currentState;

        private bool isRegexRecognize;

        public int CountSkipSymbols { get; set; }

        private IList<(string StateOut, IList<char> Value, string StateIn)> transitionMatrix;
        private IList<(string StateOut, string RegexValue, string StateIn)> transitionMatrixRegex;

        public Automation(string inputInformation, IList<(string StateOut, IList<char> Value, string StateIn)> transitionMatrix, string startState, string finishState)
        {
            this.inputInformation = inputInformation;
            this.transitionMatrix = transitionMatrix;
            this.startState = startState;
            this.finishState = finishState;
            this.currentState = this.startState;
        }

        public Automation(string inputInformation, JObject json)
        {
            if (!json["IsRegexRecognize"].Value<bool>())
            {
                IList<(string StateOut, IList<char> Value, string StateIn)> matrix = new List<(string StateOut, IList<char> Value, string StateIn)>();
                foreach (var matrixRow in json["Matrix"].AsJEnumerable())
                {
                    string stateOut = matrixRow["StateOut"].Value<string>();
                    string stateIn = matrixRow["StateIn"].Value<string>();
                    IList<char> value = matrixRow["Value"].ToObject<List<char>>();
                    matrix.Add((stateOut, value, stateIn));
                }

                this.transitionMatrix = matrix;
            }
            else
            {
                IList<(string StateOut, string RegexValue, string StateIn)> matrix = new List<(string StateOut, string RegexValue, string StateIn)>();
                foreach (var matrixRow in json["Matrix"].AsJEnumerable())
                {
                    string stateOut = matrixRow["StateOut"].Value<string>();
                    string stateIn = matrixRow["StateIn"].Value<string>();
                    string value = matrixRow["Value"].Value<string>();
                    matrix.Add((stateOut, value, stateIn));
                }
                this.transitionMatrixRegex = matrix;
                this.isRegexRecognize = true;
            }

            this.inputInformation = inputInformation;
            this.startState = json["StartState"].Value<string>();
            this.finishState = json["FinishState"].Value<string>();
            this.currentState = this.startState;

        }

        public (bool State, int CountRecognizeSymbols) Max()
        {
            currentState = startState;
            if (isRegexRecognize)
            {
                int countRecognizeSymbols = 0;

                while (true)
                {
                    if (RegexRecognizeSymbol(inputInformation, CountSkipSymbols))
                    {
                        countRecognizeSymbols++;
                        CountSkipSymbols++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (countRecognizeSymbols > 0 && currentState == finishState)
                {
                    return (true, countRecognizeSymbols);
                }
                else
                {
                    return (false, countRecognizeSymbols);
                }
            }
            else
            {
                int countRecognizeSymbols = 0;
                while (currentState != finishState)
                {
                    if (RecognizeSymbol(inputInformation, CountSkipSymbols))
                    {
                        countRecognizeSymbols++;
                        CountSkipSymbols++;
                    }
                    else
                    {
                        return (false, countRecognizeSymbols);
                    }
                }

                return (true, countRecognizeSymbols);
            }
        }

        private bool RecognizeSymbol(string inputInformation, int countSkipSymbols)
        {
            List<(string StateOut, IList<char> Value, string StateIn)> currentInformationForRecognize = transitionMatrix.ToList().FindAll(e => e.StateOut == currentState).ToList();

            (string StateOut, IList<char> Value, string StateIn) state = currentInformationForRecognize.Find(e => e.Value.Contains(inputInformation[countSkipSymbols]));

            if (state != default)
            {
                currentState = state.StateIn;
                return true;
            }

            return false;
        }

        private bool RegexRecognizeSymbol(string inputInformation, int countSkipSymbols)
        {
            if (countSkipSymbols == inputInformation.Length)
            {
                return false;
            }

            List<(string StateOut, string RegexValue, string StateIn)> currentInformationForRecognize = transitionMatrixRegex.ToList().FindAll(e => e.StateOut == currentState).ToList();

            (string StateOut, string RegexValue, string StateIn) state = currentInformationForRecognize.Find(e => Regex.IsMatch(inputInformation[countSkipSymbols].ToString(), e.RegexValue));

            if (state != default)
            {
                currentState = state.StateIn;
                return true;
            }

            return false;
        }
    }
}
