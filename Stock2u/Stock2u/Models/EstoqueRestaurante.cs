namespace Stock2u.Models
{
    public class EstoqueRestaurante
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public ICollection<Produto> Produtos { get; set; }

    }
}
