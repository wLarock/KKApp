using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KKApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileEditor fe = new FileEditor();
        string ranking = "";
        public MainWindow()
        {
            InitializeComponent();

            fe.CreateFiles();
            fillDivisionList();
        }

        private void btnAddDivision_Click(object sender, RoutedEventArgs e)
        {
            string divisionName = txtDivisionName.Text;

            if (string.IsNullOrEmpty(divisionName))
            {
                MessageBox.Show("Geen reeksnaam ingegeven", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            fe.SaveDivision(divisionName);
            txtDivisionName.Text = "";

            RefreshList();
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {
            string division = lblDivisionPlayer.Content.ToString();
            string name = txtName.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Geen naam ingegeven", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (division != "Selecteer een reeks")
            {
                fe.SavePlayer(name, division);
                txtName.Text = "";

                RefreshList();
            }
            else
                MessageBox.Show("Geen reeks geselecteerd", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnAddScore_Click(object sender, RoutedEventArgs e)
        {
            string player1 = cmbPlayer1.Text;
            string player2 = cmbPlayer2.Text;

            if (string.IsNullOrEmpty(player1) || string.IsNullOrEmpty(player2) || player1 == player2)
            {
                MessageBox.Show("Spelers niet goed geselecteerd", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (ckbWinner1.IsChecked != true && ckbWinner2.IsChecked != true)
                {
                    MessageBox.Show("Geen winnaar aangeduid", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    string gamesWon;

                    if (ckbWinner1.IsChecked ?? false)
                    {
                        gamesWon = txtGames2.Text;

                        if (string.IsNullOrEmpty(gamesWon) || !int.TryParse(gamesWon, out int n))
                        {
                            MessageBox.Show("Games foutief ingevuld", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        fe.SaveScore(player1, player2, true, int.Parse(gamesWon));
                    }
                    else
                    {
                        gamesWon = txtGames1.Text;

                        if (string.IsNullOrEmpty(gamesWon))
                        {
                            MessageBox.Show("Geen games ingevuld", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        fe.SaveScore(player1, player2, false, int.Parse(gamesWon));
                    }
                }
            }

            cmbPlayer1.Text = "";
            cmbPlayer2.Text = "";
            ckbWinner1.IsChecked = false;
            ckbWinner2.IsChecked = false;
            txtGames1.Text = "";
            txtGames2.Text = "";

            RefreshList();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ranking);
        }

        private void ckbWinner1_Checked(object sender, RoutedEventArgs e)
        {
            txtGames1.IsEnabled = false;
            ckbWinner2.IsEnabled = false;
        }

        private void ckbWinner1_Unchecked(object sender, RoutedEventArgs e)
        {
            txtGames1.IsEnabled = true;
            ckbWinner2.IsEnabled = true;
        }

        private void ckbWinner2_Checked(object sender, RoutedEventArgs e)
        {
            txtGames2.IsEnabled = false;
            ckbWinner1.IsEnabled = false;
        }

        private void ckbWinner2_Unchecked(object sender, RoutedEventArgs e)
        {
            txtGames2.IsEnabled = true;
            ckbWinner1.IsEnabled = true;
        }

        private void fillDivisionList()
        {
            string[] divisions = fe.ReadDivisions();

            for(int i = 0; i < divisions.Length; i++)
            {
                lstDivisions.Items.Add(divisions[i]);
            }
        }

        private void lstDivisions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstDivisions.Items.Count != 0)
            {
                string selectedDivision = e.AddedItems[0].ToString();
                string divisionPlayers = "";
                string names = "";
                string scores = "";
                string matches = "";

                lblDivisionScore.Content = selectedDivision;
                lblDivisionPlayer.Content = selectedDivision;

                List<Player> players = fe.ReadPlayers(selectedDivision);

                if (players != null)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        names += $"{i + 1}. {players[i].Name}\n";
                        scores += $"{players[i].score}\n";
                        matches += $"{players[i].MatchesPlayed}\n";
                        divisionPlayers += string.Format($"{i + 1}. " + "{0,-40} {1,-10} {2,-5}\n", players[i].Name, players[i].score, players[i].MatchesPlayed);
                    }
                }

                txbNames.Text = names;
                txbScores.Text = scores;
                txbMatches.Text = matches;
                ranking = divisionPlayers;

                if (players != null)
                {
                    List<Player> playersAlphabetical = players.OrderBy(p => p.Name).ToList();
                    cmbPlayer1.Items.Clear();
                    cmbPlayer2.Items.Clear();

                    for (int i = 0; i < playersAlphabetical.Count; i++)
                    {
                        cmbPlayer1.Items.Add(playersAlphabetical[i].Name);
                        cmbPlayer2.Items.Add(playersAlphabetical[i].Name);
                    }
                }
            }
            else
            {
                lblDivisionScore.Content = "Selecteer een reeks";
                lblDivisionPlayer.Content = "Selecteer een reeks";
            }
        }

        public void RefreshList()
        {
            object selectedDivision = lstDivisions.SelectedItem;
            lstDivisions.Items.Clear();
            fillDivisionList();
            lstDivisions.SelectedItem = selectedDivision;
        }
    }
}
