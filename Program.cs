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
            using (StreamReader sr = new StreamReader("/Users/matt.connley/Downloads/test.csv"))
            {       
                string currentLine;
                string firstLine = sr.ReadLine();
                var columns = firstLine.Split(",");
                columns.Skip(2).Take(columns.Length-2);
                List<string> owners = new List<string>(columns.Skip(2).Take(columns.Length-2));
                
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
                    foreach (var v in pickVals)
                    {
                        
                        testOut += " " + v;
                    }
                    Console.WriteLine(testOut);

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
    }
}
