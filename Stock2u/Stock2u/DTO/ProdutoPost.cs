namespace Stock2u.DTO
{
    public class ProdutoPost
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Avaliable { get; set; }
        public string StoragePlace { get; set; }

        public int IDEstoqueRestaurante { get; set; }
    }
}
