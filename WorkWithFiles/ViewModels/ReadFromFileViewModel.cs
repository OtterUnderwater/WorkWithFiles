using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Avalonia.Controls;
using Avalonia.Media;
using CsvHelper;
using ReactiveUI;
using WorkWithFiles.Models;
using WorkWithFiles.Views;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WorkWithFiles.ViewModels
{
	public class ReadFromFileViewModel : ReactiveObject
	{
		string directory = @"files\";
		public string? FileName { get; set; }
		public string? SelectedType { get; set; }
		public string? SelectedModel { get; set; }
		public IBrush ColorMessage { get => colorMessage; set => this.RaiseAndSetIfChanged(ref colorMessage, value); }
		IBrush colorMessage = Brushes.Red;
		public string Massage { get => massage; set => this.RaiseAndSetIfChanged(ref massage, value); }
		string massage = "";
		public List<string> TypesFiles { get => typesFiles; set => typesFiles = value; }
		List<string> typesFiles = new List<string>() { ".csv", ".json", ".xml", ".yaml" };
		public List<string> AllModels { get => allModels; set => allModels = value; }
		List<string> allModels = new List<string>() { "������", "��� � �����" };
		public List<Dogs> ListDog { get => listDog; set => listDog = value; }
		List<Dogs> listDog = new List<Dogs>();
		public List<RickAndMortys> ListPerson { get => listPerson; set => listPerson = value; }
		List<RickAndMortys> listPerson = new List<RickAndMortys>();

		/// <summary>
		/// ������ ������ �� ����� ��� ������� �� ������
		/// </summary>
		public void BtnFileRead()
		{
			ColorMessage = Brushes.Red;
			Massage = "";
			if (FileName != "" || SelectedType != "")
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
							ListDog.Add(new Dogs(1, "�����", "��������", 3, "������� ����� ���������"));
							ListDog.Add(new Dogs(2, "������", "����", 6, "���� � ������"));
							ListDog.Add(new Dogs(3, "�����", "������-�����", 2, "����� ����� ���� � ���� ������"));
							ListDog.Add(new Dogs(4, "�����", "������", 1, "����� ������ � �����"));
							DeserializationFromFile(path, ListDog);
						}
						else
						{
							ListPerson.Clear();
							ListPerson.Add(new RickAndMortys(1, "������� �����", "�����", "����"));
							ListPerson.Add(new RickAndMortys(2, "��������� �����", "���", "�������"));
							ListPerson.Add(new RickAndMortys(3, "���� �����", "���", "����"));
							DeserializationFromFile(path, ListPerson);
						}
						Massage = "������ �������� � ����: " + path;
					}
					else
					{
						Massage = "������ �� �������!";
					}
				}
				else
				{
					Massage = @"��� ����� �������� ������������ �������: /\:*?<>|";
				}
			}
			else
			{
				Massage = "����������, ������� ��� ����� � �������� ����������!";
			}
		}

		/// <summary>
		/// �������� ������������ fileName
		/// </summary>
		/// <returns>false - ���� �����������, true - ��� ������</returns>
		public bool FileNameCheck()
		{
			string forbiddenSymbols = @"/\:*?<>|";
			return !FileName.Any(forbiddenSymbols.Contains);
		}

		/// <summary>
		/// ������ � ����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="data"></param>
		public void DeserializationFromFile<T>(string path, List<T> data)
		{
			switch (SelectedType)
			{
				case ".csv": DeserializationFromCsv(path, data); break;
				case ".json": DeserializationFromJson(path, data); break;
				case ".xml": DeserializationFromXml(path, data); break;
				case ".yaml": DeserializationFromYaml(path, data); break;
			}
		}

		/// <summary>
		/// ������������ ������� � CSV � ������ � ���� � ������
		/// </summary>
		public void DeserializationFromCsv<T>(string path, List<T> data)
		{
			//�������� append � false (����������, � �� ����������)
			using (StreamWriter streamWrite = new StreamWriter(path, false, Encoding.UTF8))
			{
				using (CsvWriter csvWrite = new CsvWriter(streamWrite, CultureInfo.InvariantCulture))
				{
					csvWrite.WriteRecords(data);
				}
			}
		}

		/// <summary>
		/// ������������ ������� � JSON � ������ � ���� � ������
		/// </summary>
		public void DeserializationFromJson<T>(string path, List<T> data)
		{
			List<T> Model;
			using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					JsonSerializer serializer = new JsonSerializer();
					// ������������� JSON � ������ �������� ���� T
					Model = serializer.Deserialize<List<T>>(jsonReader);
				}
			}
		}

		/// <summary>
		/// ������������ ������� � XML � ������ � ���� � ������
		/// </summary>
		public void DeserializationFromXml<T>(string path, List<T> data)
		{
			// �������� � ����������� ��� ������ List<T>
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				xmlSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// ������������ ������� � YAML � ������ � ���� � ������
		/// </summary>
		public void DeserializationFromYaml<T>(string path, List<T> data)
		{
			//������� ������ (������������� ��������� � ������������
			//� ����� ����������� �� ������ �� ����� ����������� CamelCase
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