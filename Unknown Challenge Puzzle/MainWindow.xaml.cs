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
using System.Windows.Threading;

namespace Unknown_Challenge_Puzzle
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TimeSpan _timeRemaining = new TimeSpan(0, 15, 0);
		DispatcherTimer timer = new DispatcherTimer();
		const int Rows = 8;
		const int Columns = 13;
		const int side = 64;
		int[,] _a;
		Image[,] _pieces;
		Image _draggingImage = null;
		bool _isPlaying = false;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timerLable.Content = _timeRemaining.ToString(@"mm\:ss");
			_pieces = new Image[8, 13];
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					Image cropImage = new Image();
					CroppedBitmap cb = new CroppedBitmap(this.Resources["sourceImage"] as BitmapSource, 
						new Int32Rect(j * side, i * side, side, side));
					cropImage.Source = cb;
					gameBoard.Children.Add(cropImage);
					_pieces[i, j] = cropImage;

					cropImage.MouseDown += CropImage_MouseDown;
					cropImage.MouseUp += CropImage_MouseUp;
				}
			}
			Shuffle();

		}

		private void Shuffle()
		{
			_a = new int[8, 13];
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{

					Random rng = new Random();

					int column, row;
					do
					{
						column = rng.Next(13);
						row = rng.Next(8);
					} while (_a[row, column] != 0);

					_a[row, column] = i * Rows + j + 1;
					Grid.SetColumn(_pieces[i, j], column);
					Grid.SetRow(_pieces[i, j], row);

				}
			}
		}

		private void CropImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (_isPlaying == true)
			{
				if (_draggingImage != null)
				{
					//swaping
					var dropImage = sender as Image;
					int dropI = (int)dropImage.GetValue(Grid.RowProperty);
					int dropJ = (int)dropImage.GetValue(Grid.ColumnProperty);

					int dragI = (int)_draggingImage.GetValue(Grid.RowProperty);
					int dragJ = (int)_draggingImage.GetValue(Grid.ColumnProperty);

					Grid.SetColumn(dropImage, dragJ);
					Grid.SetRow(dropImage, dragI);

					Grid.SetColumn(_draggingImage, dropJ);
					Grid.SetRow(_draggingImage, dropI);

					int temp = _a[dropI, dropJ];
					_a[dropI, dropJ] = _a[dragI, dragJ];
					_a[dragI, dragJ] = temp;

					//check win
					if (checkWin() == true)
					{
						MessageBox.Show("You win!");
						timer.Stop();
						_isPlaying = false;
						_timeRemaining = new TimeSpan(0, 15, 0);
					}

					_draggingImage = null;
				}
			}
		}

		private void CropImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			_draggingImage = sender as Image;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{

			if(_timeRemaining.TotalSeconds == 0)
			{
				MessageBox.Show("Game Over!");
				timer.Stop();
				_isPlaying = false;
				_timeRemaining = new TimeSpan(0, 15, 0);
			}
			_timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
			timerLable.Content = _timeRemaining.ToString(@"mm\:ss");
		}

		private bool checkWin()
		{
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (_a[i, j] != i * Rows + j + 1) return false;
				}
			}
			return true;
		}

		private void playButton_Click(object sender, RoutedEventArgs e)
		{
	
			if (_isPlaying == false)
			{
				timer.Start();
				playButton.Content = "Pause";
			}
			else
			{
				timer.Stop();
				playButton.Content = "Play";
			}
			_isPlaying = !_isPlaying;

		}

		private void NewGameMenuItem_Click(object sender, RoutedEventArgs e)
		{
			_isPlaying = false;
			playButton.IsEnabled = true;
			playButton.Content = "Play";
			timer.Stop();
			Shuffle();
			_timeRemaining = new TimeSpan(0, 15, 0);
			timerLable.Content = _timeRemaining.ToString(@"mm\:ss");
		}
	}
}
