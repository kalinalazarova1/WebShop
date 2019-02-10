namespace WebShop.Models.Entities
{
    public class Photo
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public byte[] Image { get; set; }
    }
}
