using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ReactiveUI;
using WorkWithFiles.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Text.Json;
using Avalonia.Media;
using System.Linq;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace WorkWithFiles.ViewModels
{
	public class WriteToFileViewModel : ReactiveObject
	{
		string directory = @"files\";
		public string FileName { get => fileName; set => fileName = value; }
		string fileName = "";
		public string SelectedType { get => selectedType; set => selectedType = value; }
		string selectedType = "";
		public string SelectedModel { get => selectedModel; set => selectedModel = value; }
		string selectedModel = "";
		public IBrush ColorMessage { get => colorMessage; set => this.RaiseAndSetIfChanged(ref colorMessage, value); }
		IBrush colorMessage = Brushes.Red;
		public string Massage { get => massage; set => this.RaiseAndSetIfChanged(ref massage, value); }
		string massage = "";
		public List<string> TypesFiles { get => typesFiles; set => typesFiles = value; }
		List<string> typesFiles = new List<string>() { ".csv", ".json", ".xml", ".yaml" };
		public List<string> AllModels { get => allModels; set => allModels = value; }
		List<string> allModels = new List<string>() { "Собаки", "Рик и Морти" };
		public List<Dogs> ListDog { get => listDog; set => listDog = value; }
		List<Dogs> listDog = new List<Dogs>();
		public List<RickAndMortys> ListPerson { get => listPerson; set => listPerson = value; }
		List<RickAndMortys> listPerson = new List<RickAndMortys>();

		/// <summary>
		/// Записывает данные в файл при нажатии на кнопку
		/// </summary>
		public void BtnFileWrite()
		{
			ColorMessage = Brushes.Red;
			Massage = "";
			if (FileName != "" && SelectedType != "")
			{
				if (FileNameCheck())
				{
					if (SelectedModel != "")
					{
						ColorMessage = Brushes.Green;
						string path = directory + FileName + SelectedType;
						if (SelectedModel == AllModels[0])
						{
							ListDog.Clear();
							ListDog.Add(new Dogs(1, "Энджи", "Ретривер", 3, "Мечтает стать поводырем"));
							ListDog.Add(new Dogs(2, "Патрик", "Мопс", 6, "Спит и ворчит"));
							ListDog.Add(new Dogs(3, "Белла", "Бордер-колли", 2, "Любит пасти овец и мого бегать"));
							ListDog.Add(new Dogs(4, "Терра", "Пудель", 1, "Любит играть в мячик"));
							SerializationToFile(path, ListDog);
						}
						else
						{
							ListPerson.Clear();
							ListPerson.Add(new RickAndMortys(1, "Большой Морти", "Мертв", "Люди"));
							ListPerson.Add(new RickAndMortys(2, "Токсичный Морти", "Жив", "Токсины"));
							ListPerson.Add(new RickAndMortys(3, "Злой Морти", "Жив", "Люди"));
							SerializationToFile(path, ListPerson);
						}
						Massage = "Данные записаны в файл: " + path;
					}
					else
					{
						Massage = "Модель не выбрана!";
					}
				}
				else
				{
					Massage = @"Имя файла содержит недопустимые символы: /\:*?<>|";
				}
			}
			else
			{
				Massage = "Пожалуйста, введите имя файла и выберите расширение!";
			}
		}

		/// <summary>
		/// Проверка корректности fileName
		/// </summary>
		/// <returns>false - файл неккоректен, true - все хорошо</returns>
		public bool FileNameCheck()
		{
			string forbiddenSymbols = @"/\:*?<>|";
			return !FileName.Any(forbiddenSymbols.Contains);
		}

		/// <summary>
		/// Запись в файл
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="data"></param>
		public void SerializationToFile<T>(string path, List<T> data)
		{
			switch (SelectedType)
			{
				case ".csv": SerializationToCsv(path, data); break;
				case ".json": SerializationToJson(path, data); break;
				case ".xml": SerializationToXml(path, data); break;
				case ".yaml": SerializationToYaml(path, data); break;
			}
		}

		/// <summary>
		/// Сериализация объекта в CSV и запись в файл в потоке
		/// </summary>
		public void SerializationToCsv<T>(string path, List<T> data)
		{
			//аргумент append в false (перезапись, а не добавление)
			using (StreamWriter streamWrite = new StreamWriter(path, false, Encoding.UTF8))
			{
				using (CsvWriter csvWrite = new CsvWriter(streamWrite, CultureInfo.InvariantCulture))
				{
					csvWrite.WriteRecords(data);
				}
			}
		}

		/// <summary>
		/// Сериализация объекта в JSON и запись в файл в потоке
		/// </summary>
		public void SerializationToJson<T>(string path, List<T> data)
		{
			//FileMode.Create перезаписывает, а не добавляет
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				JsonSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// Сериализация объекта в XML и запись в файл в потоке
		/// </summary>
		public void SerializationToXml<T>(string path, List<T> data)
		{
			// передаем в конструктор тип класса List<T>
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				xmlSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// Сериализация объекта в YAML и запись в файл в потоке
		/// </summary>
		public void SerializationToYaml<T>(string path, List<T> data)
		{
			//Создаем объект (дополнительно указываем в соответствии
			//с каким соглашением об именах он будет именоваться CamelCase
			var yamlSerializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			using (TextWriter writer = new StreamWriter(path, false))
			{
				yamlSerializer.Serialize(writer, data, typeof(List<T>));
			}
		}
	}
}