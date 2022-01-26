using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GameOfLife.Common;
using GameOfLifeXamarin.Common;
using GameOfLifeXamarin.Enums;
using GameOfLifeXamarin.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GameOfLifeXamarin
{
    public partial class MainPage : ContentPage
    {
        public double WidthCellSize { get; set; } = 10;
        public double HeightCellSize { get; set; } = 10;
        public Board GameBoard { get; set; }
        public String BoardWidth { get; set; } = "10";
        public String BoardHeight { get; set; } = "10";

        private bool RepeatMode { get; set; }

        public ICommand ChangeStatusCommand { get; }

        private bool _advanced;
        private readonly double _xamarinWidth;

        string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.xml");
        public bool Advanced
        {
            get { return _advanced; }
            // If advanced value change, we have to recalculate board.
            set
            {
                _advanced = value;
                if (value == true)
                {
                    GameBoard.ApplyPreviousStatus();
                    GameBoard.ApplyFutureStatus();
                }
                else
                {
                    GameBoard.BoardSpecialToNormal();
                }
            }
        }
        public MainPage()
        {
            InitializeComponent();
            _xamarinWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            ChangeStatusCommand = new Command(ChangeStatus);
        }

        void Start(object sender, EventArgs args)
        {
            WidthCellSize = _xamarinWidth / int.Parse(BoardWidth);
            HeightCellSize = _xamarinWidth / int.Parse(BoardHeight);
            GameBoard = new Board(int.Parse(BoardWidth), int.Parse(BoardHeight));

            BindableLayout.SetItemsSource(BoardView, GameBoard.Fields);
            ChangeGameStatus();
        }

        void ChangeGameStatus()
        {
            BeforeGameLayout.IsVisible = !BeforeGameLayout.IsVisible;
            GameLayout.IsVisible = !GameLayout.IsVisible;

        }
        void ChangeStatus(object s)
        {
            Field field = (Field)s;
            GameBoard.AddToLast(field);
            if (field.IsAlive())
                field.FieldStatus = Status.Dead;
            else
                field.FieldStatus = Status.Alive;
        }

        private void NextTurn(object sender, EventArgs e)
        {
            GameBoard.NextGen(Advanced);
        }

        private void PreviousTurn(object sender, EventArgs e)
        {
            GameBoard.PreviousGen(Advanced);
        }

        private void NextTurnAdvanced(object sender, EventArgs e)
        {
            ChangeRepeatMode();

            Thread thread = new Thread(NextTurnAdvanced);
            thread.Start();
        }

        private void NextTurnAdvanced()
        {
            while (RepeatMode)
            {
                GameBoard.NextGen(Advanced);
                Thread.Sleep(1000);
            }
        }
        private void PreviousTurnAdvanced(object sender, EventArgs e)
        {
            ChangeRepeatMode();
            Thread thread = new Thread(PreviousTurnAdvanced);
            thread.Start();
        }

        private void PreviousTurnAdvanced()
        {
            while (RepeatMode)
            {
                GameBoard.PreviousGen(Advanced);
                Thread.Sleep(1000);
            }
        }

        private void Stop(object sender, EventArgs e)
        {
            ChangeRepeatMode();
        }

        private void ChangeRepeatMode()
        {
            RepeatMode = !RepeatMode;
            ButtonsGrid.IsVisible = !ButtonsGrid.IsVisible;
            ButtonsRepeatModeGrid.IsVisible = !ButtonsRepeatModeGrid.IsVisible;
        }

        private void Save(object sender, EventArgs e)
        {
            CommonUtils.WriteToXmlFile(_fileName, GameBoard);
        }

        private void Load(object sender, EventArgs e)
        {
            BindableLayout.SetItemsSource(BoardView, null);
            GameBoard = CommonUtils.ReadFromXmlFile<Board>(_fileName);
            WidthCellSize = _xamarinWidth / int.Parse(BoardWidth);
            HeightCellSize = _xamarinWidth / int.Parse(BoardHeight);
            BindableLayout.SetItemsSource(BoardView, GameBoard.Fields);
        }

        private async Task SaveToFile()
        {
            var result = await FilePicker.PickAsync();
        }
    }
}
