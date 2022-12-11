using System.ComponentModel.DataAnnotations;

namespace Movie_List.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required,MaxLength(250)]
        public string Title { get; set; }

        public int year { get; set; }

        public double Rate { get; set; }
        [Required,MaxLength(250)]
        public string StoreLine { get; set; }

        public byte[] poster { get; set;}

        public int GenreId { get; set;}
        public Genre genre { get; set; }
    }
}
