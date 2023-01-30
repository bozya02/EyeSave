using EyeSave.DB;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public Agent Agent { get; set; }

        public AgentPage(Agent agent, bool isNew = false)
        {
            InitializeComponent();

            Agent = agent;
        }
    }
}
