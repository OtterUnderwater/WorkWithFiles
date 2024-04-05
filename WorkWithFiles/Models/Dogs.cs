using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Models
{
	public class Dogs
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Breed { get; set; }
		public int Age { get; set; }
		public string? Description { get; set; }

		//Конструктор без параметров для десериализации
		public Dogs() { }
		public Dogs(int id, string name, string breed, int age, string description)
		{
			this.Id = id;
			this.Name = name;
			this.Breed = breed;
			this.Age = age;
			this.Description = description;
		}
	}
}
