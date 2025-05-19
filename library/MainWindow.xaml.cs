using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Collections.ObjectModel;

namespace library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<int> RatingItems { get; } = new ObservableCollection<int> { 0, 1, 2, 3, 4, 5 };
        private ObservableCollection<Book> allBooks = new ObservableCollection<Book>();

        public MainWindow()
        {
            InitializeComponent();
            LoadSampleData();
            UpdateStatistics();
        }

        private void LoadSampleData()
        {
            var book1 = new Book { Title = "Гарри Поттер", Author = "Дж. Роулинг", Genre = "Фантастика", Rating = 5, DateAdded = DateTime.Now };
            var book2 = new Book { Title = "Преступление и наказание", Author = "Ф. Достоевский", Genre = "Роман", Rating = 4, DateAdded = DateTime.Now.AddDays(-7) };

            SubscribeToBook(book1);
            SubscribeToBook(book2);

            allBooks.Add(book1);
            allBooks.Add(book2);

            LibrListLb.ItemsSource = allBooks;
        }
        private void SubscribeToBook(Book book)
        {
            book.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Book.IsDone) || e.PropertyName == nameof(Book.Rating))
                {
                    UpdateStatistics();
                }
            };
        }

        private void UpdateStatistics()
        {
            var totalBooks = allBooks.Count;
            var readCount = allBooks.Count(b => b.IsDone); 
            var avgRating = totalBooks == 0 ? 0 : Math.Round(allBooks.Average(b => b.Rating), 2);

            TotalBooksText.Text = $"Количество книг: {totalBooks}";
            ReadBooksText.Text = $"Количество прочитанных: {readCount}";
            AvgRatingText.Text = $"Средний рейтинг: {avgRating:F1}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddWindow();



            addWindow.BookAdded += book =>
            {
                SubscribeToBook(book); // подписываемся на PropertyChanged
                allBooks.Add(book);
                LibrListLb.Items.Refresh();
                UpdateStatistics(); // вызываем обновление
            };


            addWindow.Show();
        }

      

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            var filtered = allBooks.Where(b =>
                b.Title.ToLower().Contains(searchText) ||
                b.Author.ToLower().Contains(searchText) ||
                b.Genre.ToLower().Contains(searchText)
            ).ToList();

            LibrListLb.ItemsSource = filtered;
        }

        private void GenreFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGenre = (GenreFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedGenre == "Все жанры")
                ApplyFilters();
            else
                LibrListLb.ItemsSource = allBooks.Where(b => b.Genre == selectedGenre).ToList();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string status = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (status)
            {
                case "Прочитанные":
                    LibrListLb.ItemsSource = allBooks.Where(b => b.IsDone);
                    break;
                case "Не прочитанные":
                    LibrListLb.ItemsSource = allBooks.Where(b => !b.IsDone);
                    break;
                default:
                    ApplyFilters();
                    break;
            }
        }

        private void MinRatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (MinRatingComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (int.TryParse(selectedItem, out int minRating))
                LibrListLb.ItemsSource = allBooks.Where(b => b.Rating >= minRating);
            else
                ApplyFilters();
        }

        private void SortByDate_Click(object sender, RoutedEventArgs e)
        {
            LibrListLb.ItemsSource = allBooks.OrderBy(b => b.DateAdded).ToList();
        }

        private void SortByAuthor_Click(object sender, RoutedEventArgs e)
        {
            LibrListLb.ItemsSource = allBooks.OrderBy(b => b.Author).ToList();
        }

        private void SortByRating_Click(object sender, RoutedEventArgs e)
        {
            LibrListLb.ItemsSource = allBooks.OrderBy(b => b.Rating).ToList();
        }

        private void ApplyFilters()
        {
            LibrListLb.ItemsSource = allBooks;
        }
        

        

        private void LibrListLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LibrListLb.SelectedItem is Book selectedBook)
            {
                DescriptionTextBlock.Text = $"Название: {selectedBook.Title}\n" +
                                            $"Автор: {selectedBook.Author}\n" +
                                            $"Жанр: {selectedBook.Genre}\n" +
                                            $"Дата добавления: {selectedBook.DateAdded.ToShortDateString()}\n" +
                                            $"Рейтинг: {selectedBook.Rating}\n" +
                                            $"Статус: {(selectedBook.IsDone ? "Прочитана" : "Не прочитана")}";
            }
        }
    }
}
