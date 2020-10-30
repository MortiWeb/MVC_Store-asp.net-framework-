﻿using MVC_Store.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Models.ViewModels.Pages
{
    public class PageVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Article { get; set; }
        public bool HasSidebar { get; set; }
        public int Sorting { get; set; }

        public PageVM() {}
        public PageVM(PagesDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
            Article = row.Article;
        }
    }
}