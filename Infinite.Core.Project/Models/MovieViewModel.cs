using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infinite.Core.Project.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide movie name")]
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Required(ErrorMessage ="Please provide Production Name")]
        public string ProductionName { get; set; }

        [Required(ErrorMessage = "Please provide valid release date")]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [Required(ErrorMessage ="Please provide genre id")]
        [Display(Name = "Genre")]
        public int? GenreId { get; set; }
        public string GenreName { get; set; }

        public List<GenreViewModel> Genres { get; set; }
    }
}
