using EyeSave.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace EyeSave.Windows
{
    /// <summary>
    /// Interaction logic for EditPriorityWindow.xaml
    /// </summary>
    public partial class EditPriorityWindow : Window
    {
        public double Priority { get; set; }
        public List<Agent> Agents { get; set; }

        public EditPriorityWindow(List<Agent> products)
        {
            InitializeComponent();
            Agents = products;
            Priority = (double)Agents.Max(x => x.Priority);
            this.DataContext = this;
        }

        private void tbPriority_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = tbPriority.Text;

            if (text == "")
            {
                tbPriority.Text = "";
                return;
            }
            double price;
            if (double.TryParse(text, out price))
            {
                Priority = price;
                tbPriority.Text = Priority.ToString();
            }
            else
                tbPriority.Text = Priority.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            foreach (var agent in Agents)
            {
                agent.Priority += (int)Priority;
                DataAccess.SaveAgent(agent);

                AgentPriorityHistory agentPriorityHistory = new AgentPriorityHistory
                {
                    Agent = agent,
                    ChangeDate = DateTime.Now,
                    PriorityValue = agent.Priority,
                };
                DataAccess.AddPriorityHistory(agentPriorityHistory);
            }
        }
    }
}
