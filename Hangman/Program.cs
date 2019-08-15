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
        static string apikey = string.Empty;
        static void Main(string[] args)
        {
            Console.WriteLine("CLI HANGMAN - Daniel Binoy");
            string url1 = @"https://random-word-api.herokuapp.com/key?";
            HttpWebRequest apirequest = (HttpWebRequest)WebRequest.Create(url1);
            List<string> wrongletters;
            apirequest.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)apirequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                apikey = reader.ReadToEnd();
            }
        StartProgram:
            wrongletters = new List<string>();
            Console.Clear();
            Console.WriteLine("getting word...");
            string word = getWord();
            Console.WriteLine("Got word!");
            int triesleft = word.Length+20;
            string guessword = "";
            while(guessword.Length!= word.Length)
            {
                guessword += "_";
            }
            Console.Clear();
            printCurrentGuess(guessword,triesleft,wrongletters);
            Console.WriteLine("Enter difficulty level(1-20)");
            triesleft -= Convert.ToInt32(Console.ReadLine());
            while (!word.Equals(guessword) && triesleft>0) {
                Console.Clear();
                printCurrentGuess(guessword,triesleft,wrongletters);
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
                    wrongletters.Add(guessletter);
                    Console.WriteLine($"WRONG. Tries Left: {triesleft}");
                }       
            }
            if (word.Equals(guessword))
            {
                Console.WriteLine($"Correct! The word was {word}");
                Console.WriteLine("Play again? (Y/N)");
                string response = Console.ReadLine();
                if (response.Equals("Y") || response.Equals("y") || response.Equals("yes"))
                {
                    goto StartProgram;
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("You lose!");
                Console.WriteLine($"The word was: {word}");
                Console.WriteLine("Play again? (Y/N)");
                string response = Console.ReadLine();
                if (response.Equals("Y") || response.Equals("y") || response.Equals("yes"))
                {
                    goto StartProgram;
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            Console.ReadLine();
        }
        static string getWord()
        {
            string html = string.Empty;
            string url = @"https://random-word-api.herokuapp.com/word?key="+apikey+"&number=1";
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
        static void printCurrentGuess(string guessxd,int triesleftxd, List<string> wronglettersxd)
        {
            string output = "";
            for(int i = 0; i < guessxd.Length; i++)
            {
                output += (guessxd[i] + " ");
            }
            output += "                                    \n\nWRONG LETTERS: {";
            output += string.Join(", ", wronglettersxd);
            output+="}                 \n\nTRIES LEFT: ";
            output += triesleftxd;
            output += "\n\n";
            Console.WriteLine(output);
        }
    }
}
