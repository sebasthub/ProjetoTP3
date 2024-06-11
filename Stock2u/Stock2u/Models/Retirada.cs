namespace Stock2u.Models;

public class Retirada
{
    public Guid ID { get; set; }

    public int Quantity { get; set; }
    
    public int IdProduto { get; set; }
    
    public Produto Produto { get; set; }
    
}