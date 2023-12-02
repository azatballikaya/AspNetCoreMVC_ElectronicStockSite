namespace FormsApp.Models
{
    public class Sepet{
        
        public List<Product> _products { get; set; }=new List<Product>();
        public decimal TotalPrice { 
          get {
            return _products.Sum(x=>x.Price);
        }  }
    }
}