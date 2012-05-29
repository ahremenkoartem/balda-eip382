namespace Balda
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using Game;
	using Game.Entity;

	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IGameWindow
	{
		private readonly IList<Letter> word = new List<Letter>();
		private TextBox focusElement;
		private GamePresenter gamePresenter;
		private StackPanel[] stackPanels;
		private TextBox[,] textBoxs;
		private IEnumerable<Letter> letters;

		public MainWindow()
		{
			InitializeComponent();

			CreateWindowControls();
			groupBox1.Visibility = Visibility.Hidden;

			gamePresenter = new GamePresenter(this);
		}

		public IGameWindow ShowLetters(IEnumerable<Letter> letters)
		{
			this.letters = letters;
			foreach (Letter letter in letters)
			{
				TextBox textbox = textBoxs[letter.Coordinate.Y, letter.Coordinate.X];
				textbox.Text = letter.LetterSymbol.ToString();
				textbox.IsReadOnly = true;
				textbox.Background = new SolidColorBrush
				                     	{
				                     		Color = new Color {G = 255, A = 50}
				                     	};
			}

			return this;
		}

		public IGameWindow ClearField()
		{
			foreach (TextBox textBox in textBoxs)
			{
				textBox.Text = string.Empty;
				textBox.IsReadOnly = false;

				textBox.Background = new SolidColorBrush
				                     	{
				                     		Color = new Color {G = 0}
				                     	};
			}
			word.Clear();

			return this;
		}

		public event EventHandler<EventArgs> NewGame;
		public event EventHandler<EventArgs> AcceptWord;
		public event EventHandler<EventArgs> CheckWord;

		public void ShowAcceptWord(string word)
		{
			groupBox1.Visibility = Visibility.Visible;
			acceptText.Text = String.Format("Слова {0} нет в словаре, \n добавить?", word);
		}

		public void ShowPlayers(Player playerOne, Player playerTwo)
		{
			player1Name.Text = playerOne.Name;
			player2Name.Text = playerTwo.Name;

			player1Points.Text = playerOne.Points.ToString();
			player2Points.Text = playerTwo.Points.ToString();
			stackPanel1.Children.Clear();
			stackPanel2.Children.Clear();

			foreach (string solveWord in playerOne.SolveWords)
			{
				stackPanel1.Children.Add(new TextBlock {Text = solveWord});
			}

			foreach (string solveWord in playerTwo.SolveWords)
			{
				stackPanel2.Children.Add(new TextBlock {Text = solveWord});
			}
		}

		public IEnumerable<Letter> GetCurrentWord()
		{
			return word;
		}

		private void CreateWindowControls()
		{
			stackPanels = new StackPanel[5];

			for (int i = 0; i < stackPanels.Length; i++)
			{
				stackPanels[i] = new StackPanel
				                 	{
				                 		MaxHeight = 40,
				                 		MinHeight = 40,
				                 		MaxWidth = 200,
				                 		MinWidth = 200,
				                 		Orientation = Orientation.Horizontal
				                 	};

				fieldStackPanel.Children.Add(stackPanels[i]);
			}

			MakeGrid();
		}

		private void MakeGrid()
		{
			textBoxs = new TextBox[stackPanels.Count(),stackPanels.Count()];
			for (int i = 0; i < stackPanels.Count(); i++)
				for (int j = 0; j < stackPanels.Count(); j++)
				{
					textBoxs[i, j] = new TextBox
					                 	{
					                 		FontSize = 26,
					                 		Width = 40,
					                 		Height = 40,
					                 		TextAlignment = TextAlignment.Center,
					                 		FontStyle = FontStyles.Normal,
					                 		FontWeight = FontWeights.Thin,
											MaxLines = 1,
											MaxLength = 1,
					                 	};
					textBoxs[i, j].GotFocus += GotFocus;
					textBoxs[i, j].PreviewMouseLeftButtonDown += MouseDown;
					textBoxs[i, j].TextChanged += TextChanged;

					stackPanels[i].Children.Add(textBoxs[i, j]);
				}
		}

		void TextChanged(object sender, TextChangedEventArgs e)
		{
			var count = 0;
			foreach (var textbox in textBoxs)
			{
				if (textbox.Text != String.Empty)
					count++;
			}
			if (letters.Count() + 1 < count)
				(sender as TextBox).Text = string.Empty;
		}

		private new void MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is TextBox &&
			    (sender as TextBox).Text != string.Empty)
			{
				AddToWord(sender as TextBox);
			}
			else
			{
				ClearWord();
			}
		}

		private void AddToWord(TextBox textBox)
		{
			Coordinate coordinate = null;

			for (int i = 0; i < textBoxs.GetLength(0); i++)
				for (int j = 0; j < textBoxs.GetLength(1); j++)
				{
					if (textBoxs[i, j] == textBox)
					{
						coordinate = new Coordinate(j, i);
					}
				}

			Letter newletter = null;
			if (coordinate != null)
				newletter = new Letter
				            	{
				            		Coordinate = coordinate,
				            		LetterSymbol = textBox.Text.FirstOrDefault()
				            	};

			if (newletter != null && TrueLetter(newletter))
			{
				word.Add(newletter);
				ShowCurrentWord(textBox);
			}
			else
			{
				word.Clear();
				ClearWord();
				textBlock1.Text = string.Empty;
			}
		}

		private void ClearWord()
		{
			foreach (TextBox textBox in textBoxs)
			{
				if (textBox.IsReadOnly)
					textBox.Background = new SolidColorBrush
					                     	{
					                     		Color = new Color {G = 255, A = 50}
					                     	};
				else
				{
					textBox.Background = new SolidColorBrush
					                     	{
					                     		Color = new Color {G = 0}
					                     	};
				}
			}
		}

		private void ShowCurrentWord(TextBox textBox)
		{
			textBox.Background = new SolidColorBrush
			                     	{
			                     		Color = new Color {B = 255, A = 50}
			                     	};

			string wordstring = string.Empty;
			foreach (Letter letter in word)
			{
				wordstring += letter.LetterSymbol;
			}
			textBlock1.Text = wordstring;
		}

		private bool TrueLetter(Letter newletter)
		{
			bool any = (word.Any() != true);
			bool coord = false;
			if (!any)
				coord = Math.Abs((newletter.Coordinate.X - word.Last().Coordinate.X) +
				                 (newletter.Coordinate.Y - word.Last().Coordinate.Y)) == 1;

			return any || (coord && word.All(x => x.Coordinate.Equals(newletter.Coordinate) == false));
		}

		private new void GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox)
				focusElement = sender as TextBox;
		}

		private void Button1Click(object sender, RoutedEventArgs e)
		{
			if (word.Any(x => letters.Any(l => l.Equals(x)) == false))
			{
				if (CheckWord != null)
					CheckWord(sender, e);
			}
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			if (AcceptWord != null)
				AcceptWord(sender, e);
			groupBox1.Visibility = Visibility.Hidden;
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			groupBox1.Visibility = Visibility.Hidden;
		}
	}
}