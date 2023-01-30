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
        const int AGENTONPAGE = 10;
        public int PageNumber { get; set; } = 1;

        public List<Agent> Agents { get; set; }
        public List<Agent> FilteredAgents { get; set; }
        public List<AgentType> AgentTypes { get; set; }

        public Dictionary<string, Func<Agent, object>> Sortings { get; set; }

        public AgentsListPage()
        {
            InitializeComponent();

            Agents = DataAccess.GetAgents();
            FilteredAgents = Agents.ToList();
            
            AgentTypes = DataAccess.GetAgentTypes();
            AgentTypes.Insert(0, new AgentType
            {
                Title = "Все типы"
            });

            Sortings = new Dictionary<string, Func<Agent, object>>
            {
                { "Без сортировки", x => x.ID },
                { "Наименование по возрастанию", x => x.Title },
                { "Наименование по убыванию", x => x.Title },
                { "Размер скидки по возрастанию", x => x.Discount },
                { "Размер скидки по убыванию", x => x.Discount },
                { "Приоритет по возрастанию", x => x.Priority },
                { "Приоритет по убыванию", x => x.Priority },
            };

            DataAccess.NewItemAddedEvent += DataAccess_NewItemAddedEvent;
            GeneratePageNumbers();
            
            this.DataContext = this;
        }

        private void DataAccess_NewItemAddedEvent()
        {
            Agents = DataAccess.GetAgents();
            ApplyFilters(true);

            lvAgents.Items.Refresh();
        }

        private void lvAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditPriority.Visibility = lvAgents.SelectedItems == null ? Visibility.Hidden : Visibility.Visible;
        }

        private void lvAgents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var agent = (Agent)lvAgents.SelectedItem;
            if (agent != null)
                NavigationService.Navigate(new AgentPage(agent));

            lvAgents.SelectedIndex = -1;
        }

        private void btnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AgentPage(new Agent
            {
                AgentType = new AgentType()
            }, true));
        }

        private void btnEditPriority_Click(object sender, RoutedEventArgs e)
        {
            var agents = lvAgents.SelectedItems.Cast<Agent>().ToList();
            var result = new Windows.EditPriorityWindow(agents).ShowDialog();

            if (result.Value)
                MessageBox.Show("Приоритеты успешно обновлены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cbAgentTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters(bool filtersChanged = true)
        {
            var searchingText = tbSearch.Text.ToLower();
            var sorting = Sortings[cbSorting.SelectedItem as string];
            var agentType = cbAgentTypes.SelectedItem as AgentType;

            if (sorting == null || agentType == null)
                return;

            if (filtersChanged)
                PageNumber = 1;

            var t = Agents[10].Email.ToLower().Contains(searchingText);
            FilteredAgents = Agents.FindAll(agent => agent.Title.ToLower().Contains(searchingText) ||
                                                     agent.Email.ToLower().Contains(searchingText) ||
                                                     agent.Phone.ToLower().Contains(searchingText));

            if (agentType.ID != 0)
                FilteredAgents = FilteredAgents.FindAll(product => product.AgentType == agentType);

            FilteredAgents = (cbSorting.SelectedItem as string).Contains("убыванию") ?
                FilteredAgents.OrderByDescending(sorting).ToList() :
                FilteredAgents.OrderBy(sorting).ToList();


            lvAgents.ItemsSource = FilteredAgents.Skip((PageNumber - 1) * AGENTONPAGE).Take(AGENTONPAGE);

            GeneratePageNumbers();
        }

        public void GeneratePageNumbers()
        {
            Paginator.Children.Clear();

            Paginator.Children.Add(new TextBlock { Text = " < " });

            for (int i = 0, pageNumber = 1; i < FilteredAgents.Count(); i += AGENTONPAGE, pageNumber += 1)
                Paginator.Children.Add(new TextBlock {
                    Text = $" {pageNumber} "
                });

            Paginator.Children.Add(new TextBlock { Text = " > " });

            foreach (TextBlock tb in Paginator.Children)
                tb.PreviewMouseDown += PrevPage_PreviewMouseDown;
        }

        private void PrevPage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var page = (sender as TextBlock).Text;

            if (page.Contains("<"))
            {
                if (PageNumber > 1)
                    PageNumber--;
            }
            else if (page.Contains(">"))
            {
                int pageCount = FilteredAgents.Count % AGENTONPAGE == 0 ? FilteredAgents.Count / AGENTONPAGE : FilteredAgents.Count / AGENTONPAGE + 1;
                if (PageNumber + 1 < pageCount)
                    PageNumber++;
            }
            else
              PageNumber = Convert.ToInt32(page) - 1;
            
            ApplyFilters(false);
        }
    }
}
