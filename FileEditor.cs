using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKApp
{
    class FileEditor
    {
        static string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "KKApp save files");
        string playerPath = dir + @"\players.txt";
        string divisionsPath = dir + @"\divisions.txt";

        public void CreateFiles()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(playerPath))
                File.CreateText(playerPath);

            if (!File.Exists(divisionsPath))
                File.CreateText(divisionsPath);
        }

        public void SaveDivision(string divisionName)
        {
            using (StreamWriter sw = File.AppendText(divisionsPath))
            {
                sw.WriteLine(divisionName);
            }
        }

        public void SavePlayer(string playerName, string divisionName)
        {
            using(StreamWriter sw = File.AppendText(playerPath))
            {
                sw.WriteLine(playerName + "," + divisionName + ",0,0");
            }
        }

        public void SaveScore(string player1, string player2, bool player1Wins, int gamesWon)
        {
            
        }

        public string[] ReadDivisions()
        {
            if (File.ReadAllLines(divisionsPath) == null)
                return new string[0];
            else
                return File.ReadAllLines(divisionsPath);
        }
    }
}
