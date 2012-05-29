namespace Balda.Game.Entity
{
    /// <summary>
    /// Класс буквы
    /// </summary>
	public class Letter
	{
		public char LetterSymbol { get; set; }
		public Coordinate Coordinate { get; set; }
        /// <summary>
        /// Сравнение буквы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		public override bool Equals(object obj)
		{
			var o = obj as Letter;
			return LetterSymbol == o.LetterSymbol && Coordinate.Equals(o.Coordinate);
		}
	}
}