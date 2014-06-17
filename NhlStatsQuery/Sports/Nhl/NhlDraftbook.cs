using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using SportsData.Models;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("SportsDataTests")]

namespace SportsData.Nhl
{
    public class NhlDraftbook
    {
        public static List<NhlDraftbookModel> UpdateDraftbook(string fileName, [Optional] bool saveToDb)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File does not exist: {0}", fileName);
                return null;
            }

            List<NhlDraftbookModel> results = new List<NhlDraftbookModel>();

            string[] lines = File.ReadAllLines(fileName);
            for (int i = 1; i < lines.Length; i++) // start at 1 and skip the first line of headers
            {
                NhlDraftbookModel result = NhlDraftbook.ParseLine(lines[i]);
                if (null != result)
                {
                    results.Add(result);
                }
            }

            if (saveToDb == true)
            {
                NhlDraftbook.AddOrUpdateDb(results);
            }

            return results;
        }

        private static void AddOrUpdateDb(List<NhlDraftbookModel> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM NhlDraftbookModels");
                db.NhlDraftbookModels.AddRange(models);
                db.SaveChanges();
            }
        }


        /// <summary>
        /// Parse a csv line and return it as a NhlDraftBookModel
        /// </summary>
        private static NhlDraftbookModel ParseLine(string line)
        {
            string[] fields = line.Split(',');

            int expectedNumFields = 12;
            if (fields.Length != expectedNumFields)
            {
                Console.WriteLine("Expected {0} columns in line {1}", expectedNumFields, line);
                return null;
            }

            NhlDraftbookModel model = new NhlDraftbookModel();
            
            model.Year = NhlBaseClass.ConvertStringToInt(fields[0]);
            model.Round = NhlBaseClass.ConvertStringToInt(fields[1]);
            model.Pick = NhlBaseClass.ConvertStringToInt(fields[2]);
            model.Overall = NhlBaseClass.ConvertStringToInt(fields[3]);
            model.Team = fields[4];
            model.Name = fields[5];
            model.Position = fields[6];
            model.POB = fields[7];

            string height = fields[8];
            height = height.Replace(@"""", String.Empty).Replace(" ", String.Empty);
            string[] heightParts = height.Split('\'');
            if (heightParts.Length != 2)
            {
                model.HeightInches = 0;
            }
            else
            {
                model.HeightInches = NhlBaseClass.ConvertStringToInt(heightParts[0]) * 12 + NhlBaseClass.ConvertStringToInt(heightParts[1]);
            }

            model.WeightLbs = NhlBaseClass.ConvertStringToInt(fields[9]);
            model.AmateurLeague = fields[10];
            model.AmateurTeam = fields[11];

            return model;
        }
    }
}
