namespace Balda.Game.Entity
{
    /// <summary>
    /// Класс координат
    /// </summary>
	public class Coordinate
	{
		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Coordinate()
		{
		}

		public int X { get; set; }

		public int Y { get; set; }
        /// <summary>
        /// Сравнение координат
        /// </summary>
        /// <param name="other">Координата для сравнения</param>
        /// <returns>True при совпадении координаты</returns>
		public bool Equals(Coordinate other)
		{
			return other.X == X && other.Y == Y;
		}
	}
}