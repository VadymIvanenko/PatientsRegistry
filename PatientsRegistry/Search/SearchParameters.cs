using System.ComponentModel.DataAnnotations;

namespace PatientsRegistry.Search
{
    public sealed class SearchParameters
    {
        [MinLength(3)]
        [MaxLength(256)]
        public string Name { get; set; }

        [MinLength(6)]
        [MaxLength(13)]
        public string Phone { get; set; }

        [MinLength(4)]
        [MaxLength(10)]
        public string Birthdate { get; set; }
    }
}