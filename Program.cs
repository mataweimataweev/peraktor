using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TextEditor
{
    public class Figure
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Figure(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }
    }

    public class FileManager
    {
        private readonly string _filePath;

        public FileManager(string filePath)
        {
            _filePath = filePath;
        }

        public void LoadFile()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл не существует");
                return;
            }

            var extension = Path.GetExtension(_filePath);

            switch (extension)
            {
                case ".txt":
                    LoadTxtFile();
                    break;
                case ".json":
                    LoadJsonFile();
                    break;
                case ".xml":
                    LoadXmlFile();
                    break;
                default:
                    Console.WriteLine("Неподдерживаемый формат файла");
                    break;
            }
        }

        private void LoadTxtFile()
        {
            using (var reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private void LoadJsonFile()
        {
            var json = File.ReadAllText(_filePath);
            var figure = JsonConvert.DeserializeObject<Figure>(json);
            Console.WriteLine($"Name: {figure.Name}, Width: {figure.Width}, Height: {figure.Height}");
        }

        private void LoadXmlFile()
        {
            var serializer = new XmlSerializer(typeof(Figure));
            using (var reader = new StreamReader(_filePath))
            {
                var figure = (Figure)serializer.Deserialize(reader);
                Console.WriteLine($"Name: {figure.Name}, Width: {figure.Width}, Height: {figure.Height}");
            }
        }

        public void SaveFile()
        {
            var extension = Path.GetExtension(_filePath);

            switch (extension)
            {
                case ".txt":
                    SaveTxtFile();
                    break;
                case ".json":
                    SaveJsonFile();
                    break;
                case ".xml":
                    SaveXmlFile();
                    break;
                default:
                    Console.WriteLine("Неподдерживаемый формат файла");
                    break;
            }
        }

        private void SaveTxtFile()
        {
            Console.Write("Введите текст для сохранения в файле: ");
            var text = Console.ReadLine();
            File.WriteAllText(_filePath, text);
            Console.WriteLine("Файл успешно сохранен");
        }

        private void SaveJsonFile()
        {
            Console.Write("Введите название фигуры: ");
            var name = Console.ReadLine();
            Console.Write("Введите ширину фигуры: ");
            var width = int.Parse(Console.ReadLine());
            Console.Write("Введите высоту фигуры: ");
            var height = int.Parse(Console.ReadLine());

            var figure = new Figure(name, width, height);
            var json = JsonConvert.SerializeObject(figure);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Файл успешно сохранен");
        }

        private void SaveXmlFile()
        {
            Console.Write("Введите название фигуры: ");
            var name = Console.ReadLine();
            Console.Write("Введите ширину фигуры: ");
            var width = int.Parse(Console.ReadLine());
            Console.Write("Введите высоту фигуры: ");
            var height = int.Parse(Console.ReadLine());

            var figure = new Figure(name, width, height);
            var serializer = new XmlSerializer(typeof(Figure));
            using (var writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, figure);
            }
            Console.WriteLine("Файл успешно сохранен");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь к файлу: ");
            var filePath = Console.ReadLine();

            var fileManager = new FileManager(filePath);

            while (true)
            {
                Console.WriteLine("Нажмите F1 для сохранения файла или Escape для выхода");

                var key = Console.ReadKey().Key;
                Console.WriteLine();

                if (key == ConsoleKey.F1)
                {
                    fileManager.SaveFile();
                }
                else if (key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }
}