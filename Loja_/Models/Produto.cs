using System.ComponentModel.DataAnnotations;

namespace Loja_.Models
{
    public class Produto
    {
        public int ID { get; set; }
        public int OwnerID { get; set; } = 0;
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Resume { get; set; } = "";
        public string Category { get; set; } = "";
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string? Description { get; set; } = "";
        public string Phone { get; set; } = "";
        public string UserImage { get; set; } = "";
        public bool Active { get; set; } = true;
        
    }
}

