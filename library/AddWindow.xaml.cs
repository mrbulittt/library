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
using System.Windows.Shapes;

namespace library
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public delegate void BookAddedEventHandler(Book book);
        public event BookAddedEventHandler BookAdded;

        public AddWindow()
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim(); 
            string author = AuthorTextBox.Text.Trim(); 

            string genre = "Не указан";
            if (GenreComboBox.SelectedItem is ComboBoxItem item)
            {
                genre = item.Content?.ToString() ?? "Не указан";
            }

            DateTime dateAdded = DateCalendar.SelectedDate ?? DateTime.Today;

            

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            var newBook = new Book
            {
                Title = title,
                Author = author,
                Genre = genre,
                Rating = 0,
                DateAdded = dateAdded
            };

            BookAdded?.Invoke(newBook);
            this.Close();
        }
    }
}
