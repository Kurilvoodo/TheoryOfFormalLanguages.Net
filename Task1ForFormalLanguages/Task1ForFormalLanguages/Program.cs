using System;

namespace Task1ForFormalLanguages
{
    class Program
    {
        static void Main(string[] args)
        {
            FormalNumberParse parsingString = new FormalNumberParse("73"); //Because 73 is the best number in the world(c) Sheldon Cooper
            parsingString.Max();
            Console.WriteLine(parsingString.ToString() + Environment.NewLine + "Enter the any key to quit the program");
            Console.ReadKey();
        }
    }
}
