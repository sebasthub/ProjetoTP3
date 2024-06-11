using Stock2u.Models;

namespace Stock2u.DTO
{
    public class RetiradaGet
    {
        public Guid ID { get; set; }

        public ProdutosGet Produto { get; set; }

        public int quantity { get; set; }
    }
}
