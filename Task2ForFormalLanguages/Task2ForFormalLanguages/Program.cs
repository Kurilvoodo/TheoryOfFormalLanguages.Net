using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;


namespace Task2ForFormalLanguages
{
    class Program
    {
        static JObject ReadJson(string filename)
        {
            string json;
            using (StreamReader reader = new StreamReader(filename))
            {
                json = reader.ReadToEnd();
            }
            return JObject.Parse(json);
        }

        static void Main(string[] args)
        {
            List<(Automation Automation, string Name)> automationAndNames = new List<(Automation, string)>();

            string textForRecognize;

            using (StreamReader reader = new StreamReader("code.txt"))
            {
                textForRecognize = reader.ReadToEnd();
            }

            automationAndNames.Add((new Automation(textForRecognize, ReadJson("intKeyword.json")), "IntKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("booleanKeyword.json")), "BooleanKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("doubleKeyword.json")), "DoubleKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("ifKeyword.json")), "IfKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("whileKeyword.json")), "WhileKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("falseKeyword.json")), "FalseKeyword"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("whitespaces.json")), "Whitespaces"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("arithmeticOperation.json")), "ArithmeticOperation"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("logicOperation.json")), "LogicOperation"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("floatWithEValue.json")), "FloatWithEValue"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("floatValue.json")), "FloatValue"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("intValue.json")), "IntValue"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("brackets.json")), "Brackets"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("comparisonOperator.json")), "ComparisonOperator"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("varNames.json")), "VarNames"));
            automationAndNames.Add((new Automation(textForRecognize, ReadJson("serviceSymbol.json")), "ServiceSymbols"));

            JArray resultJson = new JArray();

            int countSkipSymbols = 0;
            while (countSkipSymbols != textForRecognize.Length)
            {
                bool isRecognize = false;
                foreach (var automation in automationAndNames)
                {
                    automation.Automation.CountSkipSymbols = countSkipSymbols;
                    var result = automation.Automation.Max();
                    if (result.State == true)
                    {
                        resultJson.Add(JToken.FromObject(new KeyValuePair<string, string>(automation.Name, textForRecognize.Substring(countSkipSymbols, result.CountRecognizeSymbols))));

                        countSkipSymbols += result.CountRecognizeSymbols;
                        isRecognize = true;
                        break;
                    }
                }

                if (!isRecognize)
                {
                    resultJson.Add(JToken.FromObject(new KeyValuePair<string, string>("Error recognize", countSkipSymbols.ToString())));
                    break;
                }
            }

            using (StreamWriter writer = new StreamWriter("result.json"))
            {
                writer.Write(resultJson.ToString());
            }
        }
    }
}
