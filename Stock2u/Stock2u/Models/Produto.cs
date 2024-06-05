namespace Stock2u.Models
{
    public class Produto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Avaliable { get; set; }
        public string StoragePlace { get; set; }

        public int IDEstoqueRestaurante { get; set; }

        public EstoqueRestaurante EstoqueRestaurante { get; set; } = null!;

    }
}
