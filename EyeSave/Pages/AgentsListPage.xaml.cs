using EyeSave.DB;
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

namespace EyeSave.Pages
{
    /// <summary>
    /// Interaction logic for AgentsListPage.xaml
    /// </summary>
    public partial class AgentsListPage : Page
    {
        public List<Agent> Agents { get; set; }

        public AgentsListPage()
        {
            InitializeComponent();

            Agents = DataAccess.GetAgents();

            this.DataContext = this;
        }

        private void lvAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var agent = (Agent)lvAgents.SelectedItem;
            if (agent != null)
                NavigationService.Navigate(new AgentPage(agent));

            lvAgents.SelectedIndex = -1;
        }
    }
}
