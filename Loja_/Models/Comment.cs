using Microsoft.Extensions.Hosting;

namespace Loja_.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public Produto Produto { get; set; }
    }
}
