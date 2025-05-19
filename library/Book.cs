using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace library
{
    public class Book : INotifyPropertyChanged
    {
        private int _rating;
        private bool _isDone;
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime DateAdded { get; set; }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value )
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (_isDone != value)
                {
                    _isDone = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}


