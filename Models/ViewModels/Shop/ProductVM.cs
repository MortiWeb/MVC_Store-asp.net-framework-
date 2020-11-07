using MVC_Store.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            ManufacturerId = row.ManufacturerId;
            Manufacturer = row.Manufacturer;
            StyleNumber = row.StyleNumber;
            Price = row.Price;
            ImageName = row.ImageName;
            Categories = row.Categories;
        }
        public ProductVM()
        {
            Categories = new List<CategoryDTO>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "Category name must be at least 3 and no more than 70 characters")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Description must be at least 3 characters")]
        public string Description { get; set; }
        [Required]
        public int ManufacturerId { get; set; }
        [DisplayName("Manufacturer")]
        public virtual CountryDTO Manufacturer { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "StyleNumber must be at least 3 and no more than 30 characters")]
        public string StyleNumber { get; set; }
        public decimal Price { get; set; }
        
        public string ImageName { get; set; }


        public virtual ICollection<CategoryDTO> Categories { get; set; }
        public IEnumerable<SelectListItem> DDListCategories { get; set; }
        [Required]
        [DisplayName("Select Categories")]
        public IEnumerable<int> SelectedCategoryIds { get; set; }
        public IEnumerable<SelectListItem> DDListCountries { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}