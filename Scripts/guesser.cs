using System;
using System.Collections.Generic;
using System.Linq;

namespace digit_guesser
{
    class Program
    {
        public static int GenerateCode()
        {
            var random = new Random();
            var digits = new HashSet<int>(); //must be unique

            while (digits.Count < 4)
            {
                digits.Add(random.Next(0, 10));
            }

            return int.Parse(string.Join("", digits.Select(d => d.ToString())));
        }

        static void Main(string[] args)
        {
            int code = GenerateCode();

            Console.WriteLine("Guess the code!");
            while (true)
            {
                Console.WriteLine("Enter your guess");

                var guess = Console.ReadLine();
                var count = code.ToString().ToCharArray().Intersect(guess.ToString().ToCharArray()).Count(); //(additional array of) Intersect() check what digits are in BOTH arrays
                char[] ar1 = code.ToString().ToCharArray();
                char[] ar2 = guess.ToString().ToCharArray();
                int equal = 0;
                for (int i = 0; i < ar2.Length-1; i++)
                {
                    if (ar1[i] == ar2[i]) equal++;
                }

                Console.WriteLine("{0} digits of your guess are in the code, and {1} are in the correct places.", count,equal);

                if (count == 4)
                {
                    Console.WriteLine("You guessed all the digits of the code: {0}!", code);
                    break;
                }
            }
        }
    }
}
