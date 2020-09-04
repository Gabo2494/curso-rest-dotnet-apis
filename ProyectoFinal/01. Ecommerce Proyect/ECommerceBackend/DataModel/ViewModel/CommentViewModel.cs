using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModel.ViewModel
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int OrderId { get; set; }
        public string User { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Like { get; set; }
        public string Image { get; set; }

    }
}
