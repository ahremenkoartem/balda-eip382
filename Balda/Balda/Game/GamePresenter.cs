namespace Balda.Game
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using Entity;

	public class GamePresenter
	{
		private List<Letter> field = new List<Letter>();
		private readonly IGameWindow window;
		private readonly WordDictionary wordDictionary = new WordDictionary();
		private Player currentPlayer;
		private Player playerOne;
		private Player playerTwo;


		public GamePresenter(IGameWindow window)
		{
			this.window = window;
			window.CheckWord += window_CheckWord;
			window.AcceptWord += window_AcceptWord;

			StartGame();
		}
        /// <summary>
        /// Событие принятия слова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void window_AcceptWord(object sender, EventArgs e)
		{
			AcceptWord(window.GetCurrentWord());
		}
        /// <summary>
        /// Событие проверки слова
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void window_CheckWord(object sender, EventArgs e)
		{
			CheckWord(window.GetCurrentWord());
		}
        /// <summary>
        /// Запуск игры
        /// </summary>
		private void StartGame()
		{
			playerOne = new Player("Игрок 1");
			playerTwo = new Player("Игрок 2");
			currentPlayer = playerOne;
			var word = wordDictionary.GetWord(5);
			int i = 0;
            // Записыает слово на игровое поле
			foreach (var ch in word)
			{
				field.Add(new Letter
				          	{
				          		Coordinate = new Coordinate(i++,2),
								LetterSymbol = ch
				          	});
			}
			
			ShowWindow();
		}
        /// <summary>
        /// Проверка слова в конце хода игрока
        /// </summary>
        /// <param name="word"></param>
		private void CheckWord(IEnumerable<Letter> word)
		{
			var newword = new Word(word);
			if (wordDictionary.CheckWord(newword.GetWord()))
			{
				EndStage(newword);
			}
			else
				window.ShowAcceptWord(newword.GetWord());
		}
        /// <summary>
        /// передача хода другому игроку
        /// </summary>
        /// <param name="newword"></param>
		private void EndStage(Word newword)
		{
			currentPlayer.Points += newword.GetWord().Length;
			currentPlayer.AddSolveWord(newword.GetWord());
			field.AddRange(newword.Letters);
			ChangePlayer();
			ShowWindow();
		}

		private void ShowWindow()
		{
			window
				.ClearField()
				.ShowLetters(field)
				.ShowPlayers(playerOne, playerTwo);
		}
        /// <summary>
        /// смена игрока
        /// </summary>
		private void ChangePlayer()
		{
			currentPlayer = (currentPlayer == playerOne) ? playerTwo : playerOne;
		}
        /// <summary>
        /// Добавление ранее не встречавшегося в словаре слова по желанию игрока
        /// </summary>
        /// <param name="word"></param>
		private void AcceptWord(IEnumerable<Letter> word)
		{
			var newword = new Word(word);
			wordDictionary.AddWord(newword.GetWord());
			EndStage(newword);
		}
	}
}