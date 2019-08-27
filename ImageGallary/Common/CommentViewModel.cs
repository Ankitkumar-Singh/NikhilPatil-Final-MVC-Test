using ImageGallary.Models;

namespace ImageGallary.Common
{
    public class CommentViewModel
    {
        public Image Image { get; set; }
        public Comment Comment { get; set; }
        public Tag Tag { get; set; }
    }
}