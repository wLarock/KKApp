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
            lstDivisions.Items.Clear();
            fillDivisionList();
            txtDivisionName.Text = "";
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
                fe.SavePlayer(name, division);
            else
                MessageBox.Show("Geen reeks geselecteerd", "KKApp", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnAddScore_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txbDisplay.Text);
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
            lblDivisionScore.Content = e.AddedItems[0];
            lblDivisionPlayer.Content = e.AddedItems[0];
        }
    }
}
