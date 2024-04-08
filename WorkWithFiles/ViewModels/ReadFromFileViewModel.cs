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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using YamlDotNet.Core;

namespace WorkWithFiles.ViewModels
{
	public class ReadFromFileViewModel : ReactiveObject
	{
		string directory = @"files\";
		public string FileName { get => fileName; set => fileName = value; }
		string fileName = "";
		public string SelectedType { get => selectedType; set => selectedType = value; }
		string selectedType = "";
		public string SelectedModel { get => selectedModel; set => selectedModel = value; }
		string selectedModel = "";
		public string Massage { get => massage; set => this.RaiseAndSetIfChanged(ref massage, value); }
		string massage = "";
		public List<string> TypesFiles { get => typesFiles; set => typesFiles = value; }
		List<string> typesFiles = new List<string>() { ".csv", ".json", ".xml", ".yaml" };
		public List<string> AllModels { get => allModels; set => allModels = value; }
		List<string> allModels = new List<string>() { "������", "��� � �����" };
		public List<Dogs> ListDog { get => listDog; set => this.RaiseAndSetIfChanged(ref listDog, value); }
		List<Dogs> listDog = new List<Dogs>();
		public List<RickAndMortys> ListPerson { get => listPerson; set => this.RaiseAndSetIfChanged(ref listPerson, value); }
		List<RickAndMortys> listPerson = new List<RickAndMortys>();

		/// <summary>
		/// ������ ������ �� ����� ��� ������� �� ������
		/// </summary>
		public void BtnFileRead()
		{
			Massage = "";
			if (FileName != "" && SelectedType != "")
			{
				if (FileNameCheck())
				{
					if (SelectedModel != "")
					{
							string path = directory + FileName + SelectedType;
							if (SelectedModel == AllModels[0])
							{
								DeserializationFromFile<Dogs>(path);
							}
							else
							{
								DeserializationFromFile<RickAndMortys>(path);
							}
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
		/// ������ �� �����
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="data"></param>
		public void DeserializationFromFile<T>(string path)
		{
			Massage = "";
			ListDog.Clear();
			ListPerson.Clear();
			try
			{
				switch (SelectedType)
				{
					case ".csv": DeserializationFromCsv<T>(path); break;
					case ".json": DeserializationFromJson<T>(path); break;
					case ".xml": DeserializationFromXml<T>(path); break;
					case ".yaml": DeserializationFromYaml<T>(path); break;
				}
			}
			catch
			{
				Massage = "�������� ������� �������� ������.\n���������� ������� ������!";
			}
		}

		/// <summary>
		/// �������������� �� CSV � ������ � ����
		/// </summary>
		public void DeserializationFromCsv<T>(string path)
		{
			List<T> model;
			using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
			{
				using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture, false))
				{
					model = csvReader.GetRecords<T>().ToList();
				}
			}
			PrintModel(model);
		}

		/// <summary>
		///  �������������� �� JSON � ������ � ����
		/// </summary>
		public void DeserializationFromJson<T>(string path)
		{
			List<T> model;
			using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					JsonSerializer serializer = new JsonSerializer();
					// ������������� JSON � ������ �������� ���� T
					model = serializer.Deserialize<List<T>>(jsonReader);
				}
			}
			PrintModel(model);
		}

		/// <summary>
		/// �������������� �� XML � ������ � ����
		/// </summary>
		public void DeserializationFromXml<T>(string path)
		{
			List<T> model = new List<T>();
			// ������� XmlSerializer ��� ����������� ���� �����
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
			{
				model = (List<T>)xmlSerializer.Deserialize(streamReader);
			}
			PrintModel(model);
		}

		/// <summary>
		/// �������������� �� YAML � ������ � ����
		/// </summary>
		public void DeserializationFromYaml<T>(string path)
		{
			List<T> model = new List<T>();
			// ������ ������ �� �����
			string date = File.ReadAllText(path);
			// ������� ��������������
			var yamlDeserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			// ������������� ������
			model = yamlDeserializer.Deserialize<List<T>>(date);
			PrintModel(model);
		}

		/// <summary>
		/// ��������� ���� � ��������������� ������
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		public void PrintModel<T>(List<T> data)
		{
			if (typeof(T) == typeof(Dogs))
			{
				ListDog = data.Cast<Dogs>().ToList();
			}
			else
			{
				ListPerson = data.Cast<RickAndMortys>().ToList();
			}
		}
	}
}