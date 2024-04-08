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
		List<string> allModels = new List<string>() { "������", "��� � �����" };
		public List<Dogs> ListDog { get => listDog; set => listDog = value; }
		List<Dogs> listDog = new List<Dogs>();
		public List<RickAndMortys> ListPerson { get => listPerson; set => listPerson = value; }
		List<RickAndMortys> listPerson = new List<RickAndMortys>();

		/// <summary>
		/// ���������� ������ � ���� ��� ������� �� ������
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
							ListDog.Add(new Dogs(1, "�����", "��������", 3, "������� ����� ���������"));
							ListDog.Add(new Dogs(2, "������", "����", 6, "���� � ������"));
							ListDog.Add(new Dogs(3, "�����", "������-�����", 2, "����� ����� ���� � ���� ������"));
							ListDog.Add(new Dogs(4, "�����", "������", 1, "����� ������ � �����"));
							SerializationToFile(path, ListDog);
						}
						else
						{
							ListPerson.Clear();
							ListPerson.Add(new RickAndMortys(1, "������� �����", "�����", "����"));
							ListPerson.Add(new RickAndMortys(2, "��������� �����", "���", "�������"));
							ListPerson.Add(new RickAndMortys(3, "���� �����", "���", "����"));
							SerializationToFile(path, ListPerson);
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
		/// ������������ ������� � CSV � ������ � ���� � ������
		/// </summary>
		public void SerializationToCsv<T>(string path, List<T> data)
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
		public void SerializationToJson<T>(string path, List<T> data)
		{
			//FileMode.Create ��������������, � �� ���������
			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				JsonSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// ������������ ������� � XML � ������ � ���� � ������
		/// </summary>
		public void SerializationToXml<T>(string path, List<T> data)
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
		public void SerializationToYaml<T>(string path, List<T> data)
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