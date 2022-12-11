using System.ComponentModel.DataAnnotations;

namespace Movie_List.Models
{
    public class Genre
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
