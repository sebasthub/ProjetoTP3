namespace Stock2u.DTO
{
    public class EstoqueRestauranteGet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ProdutosGet> Produtos { get; set; }

    }
}
