using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            //getting word

            Console.WriteLine("getting word...");
            string word = getWord();
            Console.WriteLine("Got word!");
            int triesleft = word.Length+20;
            
            string guessword = "";
            while(guessword.Length!= word.Length)
            {
                guessword += "_";
            }
            Console.WriteLine(guessword);
         
            Console.WriteLine("Enter difficulty level(1-20)");
            triesleft -= Convert.ToInt32(Console.ReadLine());
            while (!word.Equals(guessword) && triesleft>0) {
                Console.WriteLine("Input a guess letter");
                bool yessir = false;
                string guessletter = Console.ReadLine();
                for(int i = 0; i < word.Length; i++)
                {
                    if(word[i] == guessletter[0] && guessword[i] == '_')
                    {
                        string temp = guessword.Substring(0, i) + guessletter[0] + guessword.Substring(i + 1, guessword.Length-i-1);
                        guessword = temp;
                        yessir = true;
                    }
                    
                }
                if (!yessir)
                {
                    triesleft--;
                    Console.WriteLine($"WRONG. Tries Left: {triesleft}");
                }
                Console.WriteLine(guessword);
            }
            if (word.Equals(guessword))
            {
                Console.WriteLine($"Correct! The word was {guessword}");
            }
            else
            {
                Console.WriteLine("You lose!");
                Console.WriteLine($"The word was: {word}");
            }
            Console.ReadLine();
        }

        static string getWord()
        {
            string html = string.Empty;
            string url = @"https://random-word-api.herokuapp.com/word?key=40C4WCYZ&number=1";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            string ret;
            char[] removeThese = { '"', '"', '[', ']' };
            ret = html.Trim(removeThese);


            return ret;


        }
    }
}
