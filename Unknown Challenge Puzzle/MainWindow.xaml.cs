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
		TimeSpan timeRemaining;
		DispatcherTimer timer = new DispatcherTimer();
		const int Rows = 8;
		const int Columns = 13;
		const int side = 64;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					Image cropImage = new Image();
					CroppedBitmap cb = new CroppedBitmap(this.Resources["sourceImage"] as BitmapSource, 
						new Int32Rect(j * side, i * side, side, side));
					cropImage.Source = cb;
					gameBoard.Children.Add(cropImage);
					
				}
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if(timeRemaining.TotalSeconds == 0)
			{
				MessageBox.Show("Game Over");
				timer.Stop();
			}
			timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
			timerLable.Content = timeRemaining.ToString(@"mm\:ss");
		}

		private void playButton_Click(object sender, RoutedEventArgs e)
		{
			timeRemaining = new TimeSpan(0, 5, 0);
			timer.Start();
		}
	}
}
