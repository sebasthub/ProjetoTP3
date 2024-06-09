using Stock2u.Models;

namespace Stock2u.DTO
{
    public class ProdutosGet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Avaliable { get; set; }
        public string StoragePlace { get; set; }

    }
}
