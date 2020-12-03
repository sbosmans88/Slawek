using System;
using System.Globalization;

using System.IO;
using System.Collections;
using System.Text;


namespace Hangman
{
    class MasterClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("*****************");
            Console.WriteLine("*****Hangman*****");
            Console.WriteLine("***EU.capitals***");
            Console.WriteLine("*****************");
            MenuGame();
        }
        /// /////////////////////////////////////////////////////////
        private static void MenuGame()
        {
            Console.WriteLine("******************");
            Console.WriteLine("START GAME.... (1)");
            Console.WriteLine("SCORES.........(2)");
            Console.WriteLine("EXIT...........(3)");
            Console.WriteLine("******************");
            var chose = Console.ReadLine();
       
            switch(chose.ToString())
            {
            
                case "1":
                    PlayGame();
                    break;
                case "2":
                    CheckFile();
                    break;
                case "3":
                    System.Environment.Exit();
                    break;
                default:
                    Console.WriteLine("================================");
                    Console.WriteLine("==== Wrong choice, try again ===");

                    MenuGame();
                    break;
            }
        }

        public static void PlayGame()
        {
            int numberLife = 5;
            int countGuess = 0;
            int countLetter = 0;
            string guess = "";
            char[] guessChar;
            char[] notInWord = { '_', '_', '_', '_', '_' };


            string[] europeanCapitals = new string[] { "Paris", "Tbilisi", "Berlin", "Athens", "Budapest", "Prague", "Copenhagen", "Tallinn", "Helsinki",  "Reykjavik", "Dublin", "Rome", "Astana", "Riga", "Tirana", "Andorra la Vella", "Yerevan", "Vienna", "Baku", "Minsk", "Brussels", "Sarajevo", "Sofia", "Zagreb", "Nicosia", "Vaduz", "Vilnius", "Luxembourg", "Skopje", "Valletta", "Chisinau", "Monaco", "Podgorica", "Amsterdam", "Oslo", "Warsaw", "Lisbon", "Bucharest", "Moscow", "San Marino", "Belgrade", "Bratislava", "Ljubljana", "Madrid", "Stockholm", "Bern", "Ankara", "Kyiv", "London" };
            string[] europeanCountries = new string[europeanCapitals.Length];

            for (int i = 0; i < europeanCountries.Length; i++)
            {
                europeanCountries[i] = "";
            }

            Random rnd = new Random();
            var randomCapital = rnd.Next(0, europeanCapitals.Length); //max never be 
            var guessCity = europeanCapitals[randomCapital];
            guessCity = guessCity.ToUpper();

            string path = @"countries_and_capitals.txt";
            StreamReader SR = File.OpenText(path);
            string str = "";

            for (int i = 0; i < europeanCapitals.Length; i++)
            {
                while ((str = SR.ReadLine()) != null)
                {
                    (string country, string capital) pair = TakingNames(str);
                    if (pair.capital == europeanCapitals[i])
                    {
                        europeanCountries[i] = pair.country;
                        break;
                    }
                }
            }
            SR.Close();

            Console.WriteLine(guessCity);

            for (int i = 0; i < guessCity.Length; i++)
            {
                if (guessCity[i] == ' ')
                {
                    guess += ' ';
                }
                else
                {
                    guess += '_';
                }
            }
            guessChar = guess.ToCharArray();
            while (numberLife > 0)
            {
                if(numberLife == 1)
                {
                    Console.WriteLine("Hint: The capital of: {0}", europeanCountries[randomCapital]);
                }
                Console.WriteLine("Guess te capital:");
                Console.Write("***");
                Console.Write(guessChar);
                Console.WriteLine("Life points: {0}", numberLife);
                Console.WriteLine("***");
                Console.Write("=== Not - in word:");
                foreach (char c in notInWord)
                {
                    Console.Write(c);
                    Console.Write(" ");
                }
                Console.WriteLine("To guess the LETTER......(1)");
                Console.WriteLine("To guess the WORD........(2)");
                var letterOrWord = Console.ReadLine();

                switch (letterOrWord.ToString())
                {
                    case "1":
                        string letter = Console.ReadLine();

                        letter = letter.ToUpper();

                        while (letter.Length != 1)
                        {
                            Console.WriteLine("================================");
                            Console.WriteLine("====== It is not a LETTER ======");
                            Console.WriteLine("========== Try again ===========");
                            letter = Console.ReadLine();
                        }

                        int correctGues = 0;

                        for (int i = 0; i < guessCity.Length; i++)
                        {
                            if (guessCity[i].Equals(letter[0]))
                            {
                                guessChar[i] = letter[0];
                                correctGues++;
                                countGuess++;
                                countLetter++;
                                if (guessChar.Length == countLetter)
                                {
                                    Win(countGuess, guessCity);
                                }

                            }

                        }
                        if (correctGues == 0)
                        {
                            countGuess++;
                            notInWord[5 - numberLife] = letter[0];
                            Console.WriteLine("Wrong, please try again");
                            numberLife -= 1;
                            FinalGrpahic(numberLife + 2);
                        }
                        break;

                    case "2":
                        Console.WriteLine("Put Capital");
                        string word = Console.ReadLine();
                        word = word.ToUpper();
                        if (String.Equals(word, guessCity))
                        {
                            countGuess++;
                            Win(countGuess, guessCity);
                        }
                        else
                        {
                            Console.WriteLine("Wrong, please try again");
                            numberLife -= 2;
                            FinalGraphic(numberLife + 2);
                            countGuess++;
                        }

                        break;
                    default:
                        Console.WriteLine("Wrong, please try again");
                        break;
                }
            }
            Console.Write("Error - in word:");
            foreach (char c in notInWord)
            {
                Console.Write(c);
                Console.Write(" ");
            }
            Console.WriteLine("You lose :(");

            EndGame();

        }

        private static (string, string) TakingNames(string lineFormFile)
        {
            string country = "";
            string capital = "";
            int i = 0;
            while(lineFormFile[i] != '|')
            {
                country += lineFormFile[i];
                i++;
            }
            country = country.Remove(i-1);
            
            i += 2;    
            while(i < lineFormFile.Length)
            {
                capital += lineFormFile[i];
                i++;
            }
            return (country, capital);
        }

        /// /////////////////////////////////////////////////////////
        private static void CheckFile()
        {
            string path = @"scores.txt";
            string score = "";
            if (File.Exists(path))
            {
                StreamReader strRea = File.OpenText(path);
                Console.WriteLine("SCORES");
                while ((score = strRea.ReadLine()) != null)
                {
                    Console.WriteLine(score);
                }
                strRea.Close();
            }
            else
            {
                Console.WriteLine("1....................");
                Console.WriteLine("2....................");
                Console.WriteLine("3....................");
                Console.WriteLine("4....................");
                Console.WriteLine("5....................");
                Console.WriteLine("6....................");
                Console.WriteLine("7....................");
                Console.WriteLine("8....................");
                Console.WriteLine("9....................");
                Console.WriteLine("10...................");
            }
            MenuGame();

        }

        private static void AddToTop(string name, string date, int count, string capital)
        {
            string path = @"scores.txt";
            StreamWriter strWri;
            if(!File.Exists(path))
            {
                strWri = File.CreateText(path);
            }
            else
            {
                strWri = new StreamWriter(path, true);
            }

            string score = "";
            score += name;
            score += " | ";
            score += date;
            score += " | ";
            score += count;
            score += " | ";
            score += capital;

            Console.WriteLine(score);
            strWri.WriteLine(score);
            strWri.Close();

        }
        
        private static void Win(int count, string capital)
        {
            string name = "";
            DateTime thisDay = DateTime.Today;
            string date = thisDay.ToString("g");
            Console.WriteLine("***WINNER****");
            Console.WriteLine("Put your name");
            Console.WriteLine("***WINNER****");
            name = Console.ReadLine();
            AddToTop(name, date, count, capital);
            EndGame();
        }

        /// /////////////////////////////////////////////////////////
        private static void EndGame()
        {
            Console.WriteLine("RESTART.........(1)";
            Console.WriteLine("MENU............(2)";

            var chose = Console.ReadLine();


            switch (chose.ToString())
            {
                case "0":
                    System.Environment.Exit(0);
                    break;
                case "1":
                    PlayGame();
                    break;
                case "2":
                    MenuGame();
                    break;
            }
        }
        private static void FinalGraphic(int life)
	{
	Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}\n{5}",
            @" __________",
            @"| /      |",
            @"|/       0",
            @"|       /|\",
            @"|       / \",
            @"|_______________");
        }
        {
            switch (life.ToString())
            {
              case 0: Console.CursorTop = 5; Console.Write(" _______________"); break;
              case 1: for (var i = 1; i < 6; i++) { Console.SetCursorPosition(0, i); Console.Write("|"); } break;
              case 2: Console.Write(" __________"); break;
              case 3: Console.CursorTop = 1; Console.Write("| /\n|/"); break;
              case 4: Console.SetCursorPosition(9, 1); Console.Write("|"); break;
              case 5: Console.SetCursorPosition(9, 2); Console.Write("0"); break;
              case 6: Console.SetCursorPosition(9, 3); Console.Write("|"); break;
              case 7: Console.SetCursorPosition(8, 3); Console.Write("/"); break;
              case 8: Console.SetCursorPosition(10, 3); Console.Write(@"\"); break;
              case 9: Console.SetCursorPosition(8, 4); Console.Write("/"); break;
              case 10: Console.SetCursorPosition(10, 4); Console.Write(@" \"); break; 
              default:
                  break;
            }
        }
    }
}