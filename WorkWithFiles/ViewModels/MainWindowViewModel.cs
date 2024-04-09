using Avalonia.Controls;
using Avalonia.Media;
using CsvHelper;
using DynamicData.Kernel;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using WorkWithFiles.Models;
using WorkWithFiles.Views;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorkWithFiles.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string directory = @"files\";
        public UserControl? GetTable { get => getTable; set => this.RaiseAndSetIfChanged(ref getTable, value); }
        UserControl? getTable;
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
        public string Query { get => query; set => query = value; }
        string query = "";
        public string MassageQuery { get => massageQuery; set => this.RaiseAndSetIfChanged(ref massageQuery, value); }
        string massageQuery = "";

        public string SelectedPerson { get => selectedPerson; set => selectedPerson = value; }
        string selectedPerson = "";
        public string SelectedDog { get => selectedDog; set => selectedDog = value; }
        string selectedDog = "";
        public List<string> TypesFiles { get => typesFiles; set => typesFiles = value; }
        List<string> typesFiles = new List<string>() { ".csv", ".json", ".xml", ".yaml" };
        public List<string> AllModels { get => allModels; set => allModels = value; }
        List<string> allModels = new List<string>() { "Собаки", "Рик и Морти" };
        public List<string> SelectionDogs { get => selectionDogs; set => selectionDogs = value; }
        List<string> selectionDogs = new List<string>() { "№", "Кличка", "Порода", "Возраст" };
        public List<string> SelectionPersons { get => selectionPersons; set => selectionPersons = value; }
        List<string> selectionPersons = new List<string>() { "№", "Имя", "Статус", "Вид" };
        public List<Dogs> ListDog { get => listDog; set => this.RaiseAndSetIfChanged(ref listDog, value); }
        List<Dogs> listDog = new List<Dogs>();
        public List<RickAndMortys> ListPerson { get => listPerson; set => this.RaiseAndSetIfChanged(ref listPerson, value); }
        List<RickAndMortys> listPerson = new List<RickAndMortys>();

        /// <summary>
        /// Проверка корректности всех данных для выполнения записи или чтения
        /// </summary>
        /// <returns></returns>
        public bool GetVerification()
        {
            string forbiddenSymbols = @"/\:*?<>|";
            if (FileName != "" && SelectedType != "")
            {
                // Проверка fileName на отсутствие запрещенных символов
                if (!FileName.Any(forbiddenSymbols.Contains))
                {
                    // Проверка fileName на длину
                    if (FileName.Length < 100)
                    {
                        if (SelectedModel != "")
                        {
                            return true;
                        }
                        else
                        {
                            Massage = "Модель не выбрана!";
                            return false;
                        }
                    }
                    else
                    {
                        Massage = "Длина имени файла не должна превышать 100 символов";
                        return false;
                    }
                }
                else
                {
                    Massage = @"Имя файла содержит недопустимые символы: /\:*?<>|";
                    return false;
                }
            }
            else
            {
                Massage = "Пожалуйста, введите имя файла и выберите расширение!";
                return false;
            }
        }
       
        /// <summary>
        /// Устанавливает значение по дефолту
        /// </summary>
        public void SetDefaultLabel()
        {
            ColorMessage = Brushes.Red;
            Massage = "";
            MassageQuery = "";
            ListDog.Clear();
            ListPerson.Clear();
            GetTable = null;
        }

        /// <summary>
        /// Записывает данные в файл при нажатии на кнопку
        /// </summary>
        public void BtnWrite()
        {
            //Создаем папку, если ее нет
            Directory.CreateDirectory(directory);
            SetDefaultLabel();
            string path = directory + FileName + SelectedType;
            if (GetVerification())
            {
                if (SelectedModel == AllModels[0])
                {
                    ListDog.Add(new Dogs(1, "Энджи", "Ретривер", 3, "Мечтает стать поводырем"));
                    ListDog.Add(new Dogs(2, "Патрик", "Мопс", 6, "Спит и ворчит"));
                    ListDog.Add(new Dogs(3, "Белла", "Бордер-колли", 2, "Любит пасти овец и много бегать"));
                    ListDog.Add(new Dogs(4, "Терра", "Пудель", 1, "Любит играть в мячик"));
                    SerializationToFile(path, ListDog);
                }
                else
                {
                    ListPerson.Add(new RickAndMortys(1, "Большой Морти", "Мертв", "Люди"));
                    ListPerson.Add(new RickAndMortys(2, "Токсичный Морти", "Жив", "Токсины"));
                    ListPerson.Add(new RickAndMortys(3, "Злой Морти", "Жив", "Люди"));
                    SerializationToFile(path, ListPerson);
                }
                ColorMessage = Brushes.Green;
                Massage = "Данные записаны в файл: " + path;
            }
        }

        /// <summary>
        /// Читает данные из файла при нажатии на кнопку
        /// </summary>
        public void BtnRead()
        {
            SetDefaultLabel();
            string path = directory + FileName + SelectedType;
            if (GetVerification())
            {
                FileInfo file = new FileInfo(path);
                bool answer;
                if (file.Exists)
                {
                    if (SelectedModel == AllModels[0])
                    {
                        answer = DeserializationFromFile<Dogs>(path);
                        GetTable = DeserializationFromFile<Dogs>(path) ? new ViewListDogs() : null;
                    }
                    else
                    {
                        answer = DeserializationFromFile<RickAndMortys>(path);
                        GetTable = answer ? new ViewListPerson() : null;
                    }
                    if (!answer)
                    {
                        Massage = "Возможно выбрана неверная модель. Попробуйте выбрать другую!";
                    }
                }
                else
                {
                    Massage = "Такого файла или пути не существует. Сначала создайте файл!";
                }
            }
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
                System.Text.Json.JsonSerializer.Serialize(fs, data);
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

        /// <summary>
        /// Чтение из файла
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public bool DeserializationFromFile<T>(string path)
        {
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
                return false;
            }
            return true;
        }

        /// <summary>
        /// Десериализация из CSV и запись в лист
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
        ///  Десериализация из JSON и запись в лист
        /// </summary>
        public void DeserializationFromJson<T>(string path)
        {
            List<T> model;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    // Десериализуем JSON в список объектов типа T
                    model = serializer.Deserialize<List<T>>(jsonReader);
                }
            }
            PrintModel(model);      
        }

        /// <summary>
        /// Десериализация из XML и запись в лист
        /// </summary>
        public void DeserializationFromXml<T>(string path)
        {
            List<T> model = new List<T>();
            // Создаем XmlSerializer для Обобщенного типа листа
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                model = (List<T>)xmlSerializer.Deserialize(streamReader);
            }
            PrintModel(model);
        }

        /// <summary>
        /// Десериализация из YAML и запись в лист
        /// </summary>
        public void DeserializationFromYaml<T>(string path)
        {
            List<T> model = new List<T>();
            // Читаем данные из файла
            string date = File.ReadAllText(path);
            // Создаем десериализатор
            var yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            // Десериализуем данные
            model = yamlDeserializer.Deserialize<List<T>>(date);
            PrintModel(model);
        }

        /// <summary>
        /// Добавляет лист в соответствующую модель
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void PrintModel<T>(List<T> data)
        {
            //Вызов исключений, если вдруг модель была определена неверно
            if (typeof(T) == typeof(Dogs))
            {
                if (!(data.Cast<Dogs>().All(x => x.Age == 0) && data.Cast<Dogs>().All(x => x.Breed == null)))
                {
                    ListDog = data.Cast<Dogs>().ToList();
                }
                else
                {
                    throw new Exception();
                }
            }
            else if (typeof(T) == typeof(RickAndMortys))
            {
                if (!(data.Cast<RickAndMortys>().All(x => x.Status == null) && data.Cast<RickAndMortys>().All(x => x.Species == null)))
                {
                    ListPerson = data.Cast<RickAndMortys>().ToList();
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Сортировка по всем параметрам
        /// </summary>
        public void BtnSortModel()
        {
            //Сортировка по параметрам
            ListDog = SelectedDog == SelectionDogs[0] ? ListDog.Cast<Dogs>().OrderBy(d => d.Id).ToList() : ListDog;
            ListDog = SelectedDog == SelectionDogs[1] ? ListDog.Cast<Dogs>().OrderBy(d => d.Name).ToList() : ListDog;
            ListDog = SelectedDog == SelectionDogs[2] ? ListDog.Cast<Dogs>().OrderBy(d => d.Breed).ToList() : ListDog;
            ListDog = SelectedDog == SelectionDogs[3] ? ListDog.Cast<Dogs>().OrderBy(d => d.Age).ToList() : ListDog;
            ListPerson = SelectedPerson == SelectionPersons[0] ? ListPerson.Cast<RickAndMortys>().OrderBy(p => p.Id).ToList() : ListPerson;
            ListPerson = SelectedPerson == SelectionPersons[1] ? ListPerson.Cast<RickAndMortys>().OrderBy(p => p.Name).ToList() : ListPerson;
            ListPerson = SelectedPerson == SelectionPersons[2] ? ListPerson.Cast<RickAndMortys>().OrderBy(p => p.Status).ToList() : ListPerson;
            ListPerson = SelectedPerson == SelectionPersons[3] ? ListPerson.Cast<RickAndMortys>().OrderBy(p => p.Species).ToList() : ListPerson;
        }

        /// <summary>
        /// Поиск по имени
        /// </summary>
        public void BtnFilterModel()
        {
            BtnRead(); //Обновляем данные модели
            BtnSortModel(); //Применяем выбранную сортировку
                            //Фильтрация по Name
            ListDog = ListDog.Cast<Dogs>().Where(p => p.Name.Contains(Query)).ToList();
            ListPerson = ListPerson.Cast<RickAndMortys>().Where(p => p.Name.Contains(Query)).ToList();
            if (ListPerson.Count == 0 && ListDog.Count == 0)
            {
                MassageQuery = "По вашему запросу нет данных!";
            }
        }
    }
}
