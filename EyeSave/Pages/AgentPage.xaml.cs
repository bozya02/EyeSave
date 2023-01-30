using EyeSave.DB;
using Microsoft.Win32;
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
    /// Interaction logic for AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public Agent Agent { get; set; }
        public List<AgentType> AgentTypes { get; set; }
        public List<Product> Products { get; set; }

        public AgentPage(Agent agent, bool isNew = false)
        {
            InitializeComponent();

            Agent = agent;
            AgentTypes = DataAccess.GetAgentTypes();
            Products = DataAccess.GetProducts();

            if (isNew)
            {
                Title = $"Новый {Title}";
                btnDelete.Visibility = Visibility.Hidden;
            }
            else
                Title = $"{Title} №{Agent.ID}";

            this.DataContext = this;

            if (agent.ProductSales != null)
                btnDelete.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataAccess.IsINNnKPPUnique(Agent))
            {
                MessageBox.Show("ИНН и/или КПП не уникален", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                DataAccess.SaveAgent(Agent);
            }
            catch
            {
                MessageBox.Show("Введены некорректные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NavigationService.GoBack();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы точно хотите удалить данного агента?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;
            DataAccess.DeleteAgent(Agent);
            NavigationService.GoBack();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                var image = File.ReadAllBytes(fileDialog.FileName);
                Agent.Logo = image;

                imgLogo.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void lvProductSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var productSale = lvProductSales.SelectedItem as ProductSale;
                if (productSale == null)
                    return;
                var result = MessageBox.Show("Вы точно хотите удалить данную продажу?", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes)
                    return;
                Agent.ProductSales.Remove(productSale);
                DataAccess.DeleteProductSale(productSale);

                lvProductSales.ItemsSource = Agent.ProductSales;
                lvProductSales.Items.Refresh();
            }
            catch { }
        }

        private void cbproducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
