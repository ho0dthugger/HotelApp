using dke.file;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для PageRedactor.xaml
    /// </summary>
    public partial class PageRedactor : Page
    {
        private prakDKEEntities dbContext;
        public PageRedactor()
        {
            InitializeComponent();
            dbContext = new prakDKEEntities();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            var rooms = dbContext.Rooms.Include(r => r.StatusRooms).ToList(); // Загружаем связанные данные
            var statusRooms = dbContext.StatusRooms.ToList();

            foreach (var room in rooms)
            {
                room.StatusRoomsList = statusRooms;
                room.StatusRooms = statusRooms.FirstOrDefault(s => s.id == room.StatusRoomId); // Устанавливаем текущий статус
            }

            lstCastings.ItemsSource = rooms;
        }

        private void Redactor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var room in lstCastings.ItemsSource.Cast<Rooms>())
                {
                    if (room.id == 0) // Новый материал
                    {
                        dbContext.Rooms.Add(room);
                    }
                    else // Существующий материал
                    {
                        dbContext.Entry(room).State = EntityState.Modified;
                    }
                }

                dbContext.SaveChanges();
                MessageBox.Show("Изменения сохранены!");
                LoadMaterials(); // Обновляем список материалов
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void lstCastings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RedBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
