//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImageGallary.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Comment
    {
        [ScaffoldColumn(false)]
        public int CommentId { get; set; }
        [ScaffoldColumn(false)]
        public int ImageId { get; set; }
        [Display(Name = "Comment")]
        [Required]
        public string Comment1 { get; set; }
        [Display(Name = "Upload date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CommentDate { get; set; }
        public int UserId { get; set; }

        public virtual UserDetail UserDetail { get; set; }
        public virtual Image Image { get; set; }
    }
}