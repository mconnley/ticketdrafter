using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace ticketdrafter
{
    class Program
    {
        static string filePath;
        static void Main(string[] args)
        {
            List<owner> owners = new List<owner>();
            List<owner> halfOwners = new List<owner>();
            List<string> tiers = new List<string>();
            List<pick> picks = new List<pick>();
            List<game> games = new List<game>();
            List<ownerPreference> prefs = new List<ownerPreference>();
            bool halfOwnersPresent = false;
            int halfOwnersIndex = 0;

            GetFile();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.Delimiters = new string[]{","};
                var headers = parser.ReadFields();
                if (headers.Any(h => h.EndsWith("-HALF")))
                {
                    Console.WriteLine("There are half ownership shares.");
                    halfOwnersPresent = true;
                }
                var ownerCols = headers.Skip(2).Where(h => h.EndsWith("-HALF") == false).ToList();

                if (halfOwnersPresent)
                {
                    Console.WriteLine("Enter pick order for Half Owners:");
                    halfOwnersIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                }

                ownerCols.Insert(halfOwnersIndex, "HALF");

                var ownerCount = ownerCols.Count();

                Console.WriteLine("There are " + ownerCols.Where(o => o != "HALF") + " full owners:");
                foreach (var item in ownerCols.Where(o => o != "HALF"))
                {
                    Console.WriteLine(item);
                }
                var halfOwnerCols = headers.Where(h => h.EndsWith("-HALF")).ToList();

                Console.WriteLine("There are " + halfOwnerCols.Count + " half owners:");
                foreach (var item in halfOwnerCols)
                {
                    Console.WriteLine(item.Replace("-HALF", ""));
                }

                var i = 1;
                foreach (var c in ownerCols)
                {
                    owners.Add(new owner { ownerName = c, ownerIndex = i % ownerCount });
                    Console.WriteLine("OWNER ORDER " + i + " MODULO " + i%ownerCount + " IS " + c);
                    i++;
                }
                
                i = 1;
                foreach (var c in halfOwnerCols)
                {
                    halfOwners.Add(new owner { ownerName = c, ownerIndex = i % halfOwnerCols.Count });
                    Console.WriteLine("HALF OWNER ORDER " + i + " MODULO " + i % halfOwnerCols.Count + " IS " + c);
                    i++;
                }

                i = 1;
                var halfOwnerPickIndex = 1;
                while (!parser.EndOfData)
                {
                    var rawOwnerCols = headers.Skip(2).ToList();
                    var currentRow = parser.ReadFields();
                    int gameNum = int.Parse(currentRow[0]);

                    var prefCols = currentRow.Skip(2).ToList();
                    
                    var pi = 0;
                    
                    foreach (var pc in prefCols)
                    {
                        prefs.Add(new ownerPreference { gameNum = gameNum, ownerName = rawOwnerCols[pi], preference = int.Parse(pc) });
                        pi++;
                    }

                    var currentOwner = owners.Where(o => o.ownerIndex == i % ownerCols.Count).First();
                    var currentOwnerName = currentOwner.ownerName;

                    if(currentOwner.ownerName == "HALF")
                    {
                        currentOwnerName = halfOwners.Where(o => o.ownerIndex == halfOwnerPickIndex % 2).FirstOrDefault().ownerName;
                        halfOwnerPickIndex++;
                    }

                    pick p = new pick() { pickNum = i, ownerName = currentOwnerName, tier = currentRow[1] };
                    picks.Add(p);

                    game g = new game { gameNum = int.Parse(currentRow[0]), tier = currentRow[1], assignedOwnerName = null, picked = false };
                    
                    games.Add(g);
                    i++;
                }

                foreach (var pick in picks)
                {
                    Console.WriteLine("PICK " + pick.pickNum + " IS TIER " + pick.tier + " AND BELONGS TO " + pick.ownerName);
                    var game = (
                        from g in games
                        join p in prefs on g.gameNum equals p.gameNum
                        where g.picked == false && g.tier == pick.tier && p.ownerName == pick.ownerName
                        orderby p.preference
                        select g).FirstOrDefault();

                    game.picked = true;
                    game.assignedOwnerName = pick.ownerName;
                    Console.WriteLine(pick.ownerName + "'s highest preferred unpicked game is " + game.gameNum);
                }

                foreach (var g in games)
                {
                    Console.WriteLine("Game " + g.gameNum + " is owned by " + g.assignedOwnerName);
                }

                foreach (var g in games)
                {
                    Console.WriteLine(g.assignedOwnerName);
                }
            }
            Console.ReadLine();
        }

        static void GetFile()
        {
            Console.WriteLine("Enter path to ticket CSV:");
            filePath = @"" + Console.ReadLine();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ERROR. File " + filePath + " does not exist. Try again.");
                GetFile();
            }
        }
    }

    class owner
    {
        public string ownerName { get; set; }
        public int ownerIndex { get; set; }
    }
    class pick
    {
        public int pickNum { get; set; }
        public string ownerName { get; set; }
        public string tier { get; set; }
    }

    class ownerPreference 
    {
        public int gameNum { get; set; }
        public string ownerName { get; set; }
        public int preference { get; set; }

    }

    class game 
    {
        public int gameNum { get; set; }
        public string tier { get; set; }
        public bool picked { get; set; }
        public string assignedOwnerName { get; set; }
    }
}