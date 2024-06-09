namespace Stock2u.Models
{
    public class Retirada
    {
        public int ID { get; set; }

        public Produto Produto { get; set; }

        public int IdProduto { get; set; }

        public int quantity { get; set; }

    }
}
