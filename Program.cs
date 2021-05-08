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
            //List<Pick> picks = new List<Pick>();
            List<string> owners;
            //List<string> tiers;


            using (TextFieldParser parser = new TextFieldParser("/Users/matt.connley/Downloads/cubs2021.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.Delimiters = new string[]{","};
                var headers = parser.ReadFields();
                owners = headers.Skip(2).ToList();

                foreach (var o in owners)
                {
                    Console.WriteLine(o);
                }
                

            }
        }
    }
}