namespace WebShop.Models.Entities
{
    public enum Country
    {
        UnitedKingdom = 1,
        France = 2,
        Germany = 3
    }

    public class Address
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public AppUser Customer { get; set; }

        public string AddresLine1 { get; set; }

        public string AddresLine2 { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public Country Country { get; set; }
    }
}
