using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Models
{
	public class RickAndMortys
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Status { get; set; }
		public string? Species { get; set; }
		public RickAndMortys() { }
		public RickAndMortys(int id, string name, string status, string species)
		{
			this.Id = id;
			this.Name = name;
			this.Status = status;
			this.Species = species;
		}
	}
}
