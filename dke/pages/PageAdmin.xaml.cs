using dke.file;
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

namespace dke.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        private prakDKEEntities dbContext;
        public PageAdmin()
        {
            InitializeComponent();
            dbContext = new prakDKEEntities();
            LoadCastings();
            btnClose.Click += btnClose_Click;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void LoadCastings()
        {
            lstCastings.ItemsSource = dbContext.Rooms.ToList();
        }

        private void lstCastings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCastings.SelectedItem != null)
            {
                Rooms selectedCasting = (Rooms)lstCastings.SelectedItem;
                // Load required materials and check availability
            }
        }

        private void ViewCustomers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageClients());
        }

        private void ViewDefects_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageContent());
        }

        private void ViewFinishedProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageContent());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageLogin());
        }

        private void redactMat_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageAdmin());
        }

        private void redactUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageAdminEmloyees());
        }

        private void domload_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageUnblockAdmin());
        }

        private void prosmGotProd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageTasksAdmin());
        }

        private void Redactor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageRedactor());
        }
    }
}
