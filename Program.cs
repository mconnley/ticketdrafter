using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ticketdrafter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Pick> picks = new List<Pick>();
            List<string> owners;
            using (StreamReader sr = new StreamReader("/Users/matt.connley/Downloads/test.csv"))
            {       
                string currentLine;
                string firstLine = sr.ReadLine();
                var columns = firstLine.Split(",");
                columns.Skip(2).Take(columns.Length-2);
                owners = new List<string>(columns.Skip(2).Take(columns.Length-2));
                
                var ownersString = "Owners are ";


                foreach (var owner in owners)
                {
                    ownersString += owner + " ";
                }

                Console.WriteLine(ownersString);

                

                while((currentLine = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(currentLine);
                    var vals = currentLine.Split(",");
                    var gameNum = vals[0];
                    var tierName = vals[1];
                    var pickVals = vals.Skip(2).Take(vals.Length-2);

                    var testOut = "Game " + gameNum + " Tier " + tierName;
                    var i = 0;

                    List<OwnerPick> ownerPicks = new List<OwnerPick>();

                    foreach (var v in pickVals)
                    {
                        var owner = owners[i];
                        testOut += " " + owner + " " + v;
                        i++;
                        OwnerPick o = new OwnerPick() { ownerName = owner, pickPreference = int.Parse(v) };
                        ownerPicks.Add(o);
                    }
                    //Console.WriteLine(testOut);
                    picks.Add(new Pick { gameNumber = int.Parse(gameNum), tier = tierName, preference = ownerPicks, picked = false });
                }
                
            }
            var tiers = picks.Select(p => p.tier).Distinct();
            foreach (var tier in tiers)
            {
                foreach (var o in owners)
                {
                    //var p = picks.OrderBy(x => x.
                    
                }
                foreach (var p in picks.Where(p => p.tier == tier))
                {
                    Console.WriteLine(p.gameNumber + " " + p.tier);
                    foreach (var op in p.preference)
                    {
                        Console.WriteLine("---- " + op.ownerName + ": " + op.pickPreference);
                    }
                
                }
            }




            //Console.ReadKey(true);

        }
    }



    class OwnerPick
    {
        public string ownerName { get; set; }
        public int pickPreference { get; set; }
    }

    class Pick
    {
        public int gameNumber { get; set; }
        public string tier { get; set; }
        public List<OwnerPick> preference { get; set; }
        public bool picked { get; set; }
        public string AssignedTo { get; set; }
    }
}
