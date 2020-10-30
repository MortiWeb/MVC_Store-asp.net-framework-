using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Store.Models.Data
{
    [Table("tblProducts")]
    public class ProductDTO
    {
        public ProductDTO()
        {
            Categories = new List<CategoryDTO>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }
        public string StyleNumber { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; }
        public virtual ICollection<CategoryDTO> Categories { get; set; }
    }
}