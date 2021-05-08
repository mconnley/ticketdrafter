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
        static void Main(string[] args)
        {
            List<owner> owners = new List<owner>();
            List<string> tiers = new List<string>();
            List<pick> picks = new List<pick>();
            List<game> games = new List<game>();
            List<ownerPreference> prefs = new List<ownerPreference>();

            //TODO: change to input param
            using (TextFieldParser parser = new TextFieldParser("d:\\MyLibraries\\Downloads\\cubs2021.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.Delimiters = new string[]{","};
                var headers = parser.ReadFields();
                var ownerCols = headers.Skip(2).ToList();
                var ownerCount = ownerCols.Count;
                Console.WriteLine("THERE ARE " + ownerCount + " owners");

                var i = 1;
                foreach (var c in ownerCols)
                {
                    owners.Add(new owner { ownerName = c, ownerIndex = i%ownerCount });
                    Console.WriteLine("OWNER ORDER " + i + " INDEX " + i%ownerCount + " IS " + c);
                    i++;
                }
               
                i = 1;
                while (!parser.EndOfData)
                {
                    var currentRow = parser.ReadFields();
                    int gameNum = int.Parse(currentRow[0]);

                    var prefCols = currentRow.Skip(2).ToList();
                    
                    var pi = 0;
                    foreach (var pc in prefCols)
                    {
                        prefs.Add(new ownerPreference { gameNum = gameNum, ownerName = owners[pi].ownerName, preference = int.Parse(pc) });
                        pi++;
                    }

                    pick p = new pick() { pickNum = i, ownerName = owners.Where(o => o.ownerIndex == i% ownerCols.Count).First().ownerName, tier = currentRow[1] };
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