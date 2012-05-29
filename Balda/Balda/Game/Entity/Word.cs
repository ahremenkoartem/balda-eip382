namespace Balda.Game.Entity
{
	using System.Collections.Generic;
	using System.Linq;
    /// <summary>
    /// Класс слова
    /// </summary>
	public class Word
	{
		private readonly List<Letter> letters = new List<Letter>();

		public Word()
		{
			
		}
        /// <summary>
        /// Конструктор с перечеслителем для побуквенного составления слова
        /// </summary>
        /// <param name="word"></param>
		public Word(IEnumerable<Letter> word)
		{
			letters.AddRange(word);
		}
        /// <summary>
        /// Добавляем буквы в коллекцию для составления слова
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
		public Word AppendLetter(Letter letter)
		{
			letters.Add(letter);
			return this;
		}
        /// <summary>
        /// Добавляет букву в коллекцию по координатам
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		public Word AppendLetter(char letter, int x, int y)
		{
			return AppendLetter(new Letter
			            	{
			            		Coordinate = new Coordinate {X = x, Y = y},
			            		LetterSymbol = letter
			            	});
		}

		public string GetWord()
		{
			return letters.Aggregate(string.Empty, (current, letter) => current + letter.LetterSymbol);
		}

		public Letter GetLetter(int x, int y)
		{
			return letters.SingleOrDefault(l => l.Coordinate.X == x && l.Coordinate.Y == y);
		}

		public Letter GetLetter(Coordinate coordinate)
		{
			return GetLetter(coordinate.X, coordinate.Y);
		}

		public IEnumerable<Letter> Letters { get { return letters; } }

		public Word ClearWorld()
		{
			letters.Clear();
			return this;
		}
	}
}