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
            string[] players = File.ReadAllLines(playerPath);
            string winner = "";
            string loser = "";
            string tempFile = Path.GetTempFileName();

            if (player1Wins)
            {
                winner = Array.Find(players, p => p.Contains(player1));
                loser = Array.Find(players, p => p.Contains(player2));
            }
            else
            {
                winner = Array.Find(players, p => p.Contains(player2));
                loser = Array.Find(players, p => p.Contains(player1));
            }

            using (StreamReader sr = new StreamReader(playerPath))
            using (StreamWriter sw = new StreamWriter(tempFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != winner && line != loser)
                    {
                        sw.WriteLine(line);
                    }
                }

                string[] winnerArray = winner.Split(',');
                string newPlayer1 = $"{winnerArray[0]},{winnerArray[1]},{int.Parse(winnerArray[2]) + 15},{int.Parse(winnerArray[3]) + 1}";
                string[] loserArray = loser.Split(',');
                string newPlayer2 = $"{loserArray[0]},{loserArray[1]},{int.Parse(loserArray[2]) + gamesWon},{int.Parse(loserArray[3]) + 1}";

                sw.WriteLine(newPlayer1);
                sw.WriteLine(newPlayer2);
            }

            File.Delete(playerPath);
            File.Move(tempFile, playerPath);
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
                    player.score = int.Parse(currentplayer[2]);
                    player.MatchesPlayed = int.Parse(currentplayer[3]);
                    sortedPlayers.Add(player);
                }
                return sortedPlayers.OrderByDescending(p => p.MatchesPlayed).OrderByDescending(p => p.score).ToList();
            }
        }
    }
}
