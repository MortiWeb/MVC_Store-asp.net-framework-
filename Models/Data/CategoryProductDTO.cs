using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Store.Models.Data
{
    [Table("tblCategoryProduct")]
    public class CategoryProductDTO
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }

        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }
    }
}