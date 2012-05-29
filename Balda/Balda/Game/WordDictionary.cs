namespace Balda.Game
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
    /// <summary>
    /// Класс словаря
    /// </summary>
	public class WordDictionary
	{
		List<string> words = new List<string>();

		public WordDictionary()
		{
			if (System.IO.File.Exists(@"words.xml"))
				LoadFromFile();
			else
			{
				words.Add("БАЛДА");
				words.Add("СУП");
				words.Add("КРЮК");
				words.Add("СПОРТ");
				SaveToFile();
			}
		}
        /// <summary>
        /// Добавляет слова в словарь
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
		 public WordDictionary AddWord(string word)
		 {
		 	if (words.All(x => x != word))
				words.Add(word);
			 SaveToFile();
			 return this;
		 }
        /// <summary>
        /// Получает слова из словаря
        /// </summary>
        /// <param name="wordLength"></param>
        /// <returns></returns>
		public string GetWord(int wordLength)
		{
			return words
				.Where(x => x.Length == wordLength)
				.Skip(new Random().Next(0,words.Count(x => x.Length == wordLength)-1))
				.FirstOrDefault();
		}
        /// <summary>
        /// Сравнение слов
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
		public bool CheckWord(string word)
		{
			return words.Any(x => x == word);
		}

		void LoadFromFile()
		{
			var reader = new System.Xml.Serialization.XmlSerializer(typeof(List<string>));
			var file = new System.IO.StreamReader(@"words.xml");
			words = new List<string>();
			words = (List<string>)reader.Deserialize(file);
		}

		void SaveToFile()
		{
			System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<string>));
			//System.IO.File.Create(@"words.xml");
			System.IO.StreamWriter file = new System.IO.StreamWriter(@"words.xml");
			writer.Serialize(file, words);
			file.Close();
		}

	}
}