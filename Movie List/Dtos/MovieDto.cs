using Movie_List.Models;
using System.ComponentModel.DataAnnotations;

namespace Movie_List.Dtos
{
    public class MovieDto
    {
        public int id { get; set; }
        public string Title { get; set; }

        public int year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }
        [Required, StringLength(250)]
        public string StoreLine { get; set; }
        [Display(Name = "Select Poster ....")]
        public byte[] poster { get; set; }
        [Display (Name ="Genre")]
        public int GenreId { get; set; }
        public IEnumerable<Genre>Genres { get; set; }
    }
}
