using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Store.Models.Data
{
    [Table("Countries")]
    public class CountryDTO
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}