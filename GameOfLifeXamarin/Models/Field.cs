using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameOfLifeXamarin.Enums;

namespace GameOfLifeXamarin.Models
{
    public class Field : INotifyPropertyChanged
    {
        private Status _status { get; set; } = Status.Dead;
        public Status FieldStatus
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged(nameof(FieldStatus));
            }
        }

        /// <summary>
        /// Check if object is alive.
        /// </summary>
        /// <returns>true if alive, false otherwise</returns>
        public bool IsAlive()
        {
            return (FieldStatus == Status.Alive) ||
                    (FieldStatus == Status.WillDie) ||
                    (FieldStatus == Status.Born);
        }

        /// <summary>
        /// Cast advanced mode to normal one.
        /// </summary>
        public void SpecialToNormal()
        {
            if (IsAlive())
            {
                FieldStatus = Status.Alive;
            }
            else
            {
                FieldStatus = Status.Dead;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
