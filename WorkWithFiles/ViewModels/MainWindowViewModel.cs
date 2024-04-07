using Avalonia.Controls;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using WorkWithFiles.Models;
using WorkWithFiles.Views;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WorkWithFiles.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		UserControl swichUC = new WorkWithFiles.Views.Menu();
		public UserControl SwichUC { get => swichUC; set => this.RaiseAndSetIfChanged(ref swichUC, value); }

		WriteToFileViewModel writeToFileVM = new WriteToFileViewModel();

		ReadFromFileViewModel readFromFileVM = new ReadFromFileViewModel();
		public WriteToFileViewModel WriteToFileVM { get => writeToFileVM; set => writeToFileVM = value; }
		public ReadFromFileViewModel ReadFromFileVM { get => readFromFileVM; set => readFromFileVM = value; }
		
		public void BtnMenu()
		{
			SwichUC = new WorkWithFiles.Views.Menu();
		}

		public void BtnWrite()
		{
			SwichUC = new WriteToFile();
		}

		public void BtnRead()
		{
			SwichUC = new ReadFromFile();
		}
	}
}
