namespace Balda.Game.Entity
{
    ///Класс "Игрок"
	using System.Collections.Generic;

	public class Player
	{
		public string Name { get; private set; }
        /// <summary>
        /// Список написанных игроком слов
        /// </summary>
		public List<string> solveWords = new List<string>();
        /// <summary>
        /// Конструктор класса "Игрок", задается имя игрока
        /// </summary>
        /// <param name="name"></param>
		public Player(string name)
		{

			this.Name = name;
		}

        /// <summary>
        /// Очки
        /// </summary>
		public int Points { get; set; }
        /// <summary>
        /// Добавляет написанное слово в список слов игрока
        /// </summary>
        /// <param name="word"></param>
		internal void AddSolveWord(string word)
		{
			solveWords.Add(word);
		}
        /// <summary>
        /// Индекс/перечислитель для списка строк с написанными словами
        /// </summary>
		public IEnumerable<string> SolveWords { get { return solveWords; } }
	}
}