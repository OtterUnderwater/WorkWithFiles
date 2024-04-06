using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ReactiveUI;
using WorkWithFiles.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Text.Json;
using WorkWithFiles.Views;
using Avalonia.Controls;

namespace WorkWithFiles.ViewModels
{
	public class WriteToFileViewModel : ReactiveObject
	{
		string directory = @"files\";
		string typeFile = "";
		string fileName = "";
		string massage = "";
		public string FileName { get => fileName; set => fileName = value; }
		public string TypeFile { get => typeFile; set => typeFile = value; }

		List<string> typesFiles = new List<string>() { ".csv",".json",".xml", ".yaml" };
		public List<string> TypesFiles { get => typesFiles; set => typesFiles = value; }
		public string Massage { get => massage; set => this.RaiseAndSetIfChanged(ref massage, value); }
		public void BtnFileWrite()
		{
			string path = directory + FileName + TypeFile;
			Massage = "������ ���� �������� � ����: " + path;
			/*
						string CSVpath1 = @"files\file1.csv";
						string CSVpath2 = @"files\file2.csv";
						string JSONpath1 = @"files\file1.json";
						string JSONpath2 = @"files\file2.json";
						string XMLpath1 = @"files\file1.xml";
						string XMLpath2 = @"files\file2.xml";
						string YAMLpath1 = @"files\file1.yaml";
						string YAMLpath2 = @"files\file2.yaml";
						ListDog.Add(new Dogs(1, "�����", "��������", 3, "������� ����� ���������"));
						ListDog.Add(new Dogs(2, "������", "����", 6, "���� � ������"));
						ListDog.Add(new Dogs(3, "�����", "������-�����", 2, "����� ����� ���� � ���� ������"));
						ListDog.Add(new Dogs(4, "�����", "������", 1, "����� ������ � �����"));
						ListPerson.Add(new RickAndMortys(1, "������� �����", "�����", "����"));
						ListPerson.Add(new RickAndMortys(2, "��������� �����", "���", "�������"));
						ListPerson.Add(new RickAndMortys(3, "���� �����", "���", "����"));
						SerializationToCsv(CSVpath1, ListDog);
						SerializationToCsv(CSVpath2, ListPerson);
						SerializationToJson(JSONpath1, ListDog);
						SerializationToJson(JSONpath2, ListPerson);
						SerializationToXml(XMLpath1, ListDog);
						SerializationToXml(XMLpath2, ListPerson);
						SerializationToYaml(YAMLpath1, ListDog);
						SerializationToYaml(YAMLpath2, ListPerson);*/
		}


		List<Dogs> listDog = new List<Dogs>();
		List<RickAndMortys> listPerson = new List<RickAndMortys>();
		public List<Dogs> ListDog { get => listDog; set => listDog = value; }
		public List<RickAndMortys> ListPerson { get => listPerson; set => listPerson = value; }
	
		/// <summary>
		/// ������������ ������� � CSV � ������ � ���� � ������
		/// </summary>
		static void SerializationToCsv<T>(string path, List<T> data)
		{
		}

		/// <summary>
		/// ������������ ������� � JSON � ������ � ���� � ������
		/// </summary>
		static void SerializationToJson<T>(string path, List<T> data)
		{
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				JsonSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// ������������ ������� � XML � ������ � ���� � ������
		/// </summary>
		static void SerializationToXml<T>(string path, List<T> data)
		{
			// �������� � ����������� ��� ������ List<T>
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				xmlSerializer.Serialize(fs, data);
			}
		}

		/// <summary>
		/// ������������ ������� � YAML � ������ � ���� � ������
		/// </summary>
		static void SerializationToYaml<T>(string path, List<T> data)
		{
			//������� ������ (������������� ��������� � ������������
			//� ����� ����������� �� ������ �� ����� ����������� CamelCase
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