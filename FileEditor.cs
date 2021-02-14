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
            string[] divisions = File.ReadAllLines(divisionsPath);

            if (divisions == null)
                return new string[0];
            else
                return divisions;
        }

        public List<Player> ReadPlayers(string division)
        {
            string[] players = File.ReadAllLines(playerPath).Where(p => p.Contains(division)).ToArray();
            List<Player> sortedPlayers = new List<Player>();
            

            if (players == null)
                return sortedPlayers;
            else
            {
                for(int i = 0; i < players.Length; i++)
                {
                    string[] currentplayer = players[i].Split(',');
                    Player player = new Player();

                    player.Name = currentplayer[0];
                    player.Division = currentplayer[1];
                    player.score = Int32.Parse(currentplayer[2]);
                    player.MatchesPlayed = Int32.Parse(currentplayer[3]);
                    sortedPlayers.Add(player);
                }
                return sortedPlayers.OrderByDescending(p => p.MatchesPlayed).OrderByDescending(p => p.score).ToList();
            }
        }
    }
}
