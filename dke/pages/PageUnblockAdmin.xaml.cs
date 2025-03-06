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
    public partial class PageUnblockAdmin : Page
    {
        private prakDKEEntities dbContext;
        public PageUnblockAdmin()
        {
            InitializeComponent();
            dbContext = new prakDKEEntities();
            LoadUsers();
        }
        private void LoadUsers()
        {
            var users = dbContext.Users.ToList();
            dgUsers.ItemsSource = users;
        }

        private void btnUnblock_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as Users;
            if (selectedUser != null)
            {
                selectedUser.Blocked = false;
                dbContext.SaveChanges();
                LoadUsers();
            }
        }

        private void btnBlock_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as Users;
            if (selectedUser != null)
            {
                selectedUser.Blocked = true;
                dbContext.SaveChanges();
                LoadUsers();
            }
        }
    }
}
