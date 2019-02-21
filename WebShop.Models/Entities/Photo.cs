using System.ComponentModel;

namespace WebShop.Models.Entities
{
    public class Photo
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        public int ProductId { get; set; }

        public byte[] Image { get; set; }
    }
}
