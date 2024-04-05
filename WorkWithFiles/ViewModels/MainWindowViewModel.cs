using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using WorkWithFiles.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WorkWithFiles.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public void BtnWrite()
		{
			//Путь до папки с файлами
			string directory = @"files\";

			/*int act;
            string namefile = "";
            int type;
			// Метод считывания/записи, должен быть универсальным для любой модели(обобщенные классы);
			 Console.WriteLine("Выберите действие с файлом:");
			 Console.WriteLine("1. Чтение");
			 Console.WriteLine("2. Запись");
			 act = Convert.ToInt32(Console.ReadLine());
			 switch (act)
			 {
				 case 1: break;
				 case 2: break;
				 default: Console.WriteLine("Такого действия нет"); break;
			 }
			 Console.WriteLine("Введите имя файла:");
			 namefile = Console.ReadLine();
			 Console.WriteLine("Выберите тип файла:");
			 Console.WriteLine("1. CSV");
			 Console.WriteLine("2. JSON");
			 Console.WriteLine("3. XML");
			 Console.WriteLine("4. YAML");
			 type = Convert.ToInt32(Console.ReadLine());
			 switch (type)
			 {
				 case 1: namefile += ".csv"; break;
				 case 2: namefile += ".json"; break;
				 case 3: namefile += ".xml"; break;
				 case 4: namefile += ".yaml"; break;
				 default: Console.WriteLine("Такого типа нет"); break;
			 }
			 string path = @"Files\" + namefile;*/

			string CSVpath1 = @"files\file1.csv";
			string CSVpath2 = @"files\file2.csv";
			string JSONpath1 = @"files\file1.json";
			string JSONpath2 = @"files\file2.json";
			string XMLpath1 = @"files\file1.xml";
			string XMLpath2 = @"files\file2.xml";
			string YAMLpath1 = @"files\file1.yaml";
			string YAMLpath2 = @"files\file2.yaml";

			ListDog.Add(new Dogs(1, "Энджи", "Ретривер", 3, "Мечтает стать поводырем"));
			ListDog.Add(new Dogs(2, "Патрик", "Мопс", 6, "Спит и ворчит"));
			ListDog.Add(new Dogs(3, "Белла", "Бордер-колли", 2, "Любит пасти овец и мого бегать"));
			ListDog.Add(new Dogs(4, "Терра", "Пудель", 1, "Любит играть в мячик"));
			ListPerson.Add(new RickAndMortys(1, "Большой Морти", "Мертв", "Люди"));
			ListPerson.Add(new RickAndMortys(2, "Токсичный Морти", "Жив", "Токсины"));
			ListPerson.Add(new RickAndMortys(3, "Злой Морти", "Жив", "Люди"));

			SerializationToCsv(CSVpath1, ListDog);
			SerializationToCsv(CSVpath2, ListPerson);
			SerializationToJson(JSONpath1, ListDog);
			SerializationToJson(JSONpath2, ListPerson);
			SerializationToXml(XMLpath1, ListDog);
			SerializationToXml(XMLpath2, ListPerson);
			SerializationToYaml(YAMLpath1, ListDog);
			SerializationToYaml(YAMLpath2, ListPerson);

		}

		List<Dogs> listDog = new List<Dogs>();
		List<RickAndMortys> listPerson = new List<RickAndMortys>();
		public List<Dogs> ListDog { get => listDog; set => listDog = value; }
		public List<RickAndMortys> ListPerson { get => listPerson; set => listPerson = value; }

		/// <summary>
		/// Сериализация объекта в CSV и запись в файл в потоке
		/// </summary>
		static void SerializationToCsv<T>(string path, List<T> data)
		{
		}

		/// <summary>
		/// Сериализация объекта в JSON и запись в файл в потоке
		/// </summary>
		static void SerializationToJson<T>(string path, List<T> data)
		{
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				JsonSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// Сериализация объекта в XML и запись в файл в потоке
		/// </summary>
		static void SerializationToXml<T>(string path, List<T> data)
		{
			// передаем в конструктор тип класса List<T>
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				xmlSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// Сериализация объекта в YAML и запись в файл в потоке
		/// </summary>
		static void SerializationToYaml<T>(string path, List<T> data)
		{
			//Создаем объект (дополнительно указываем в соответствии
			//с каким соглашением об именах он будет именоваться CamelCase
			var yamlSerializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			using (TextWriter writer = new StreamWriter(path))
			{
				yamlSerializer.Serialize(writer, data, typeof(List<T>));
			}
		}

	}
}
