using System.Reflection.Metadata;

namespace FormsApp.Models{
    public class Repository{
        private static readonly List<Product> _products=new();
        private static readonly List<Category> _categories=new();
        static Repository(){
           _categories.Add(new Category{
            CategoryId=1,
            Name="Telefon"
           });
           _categories.Add(new Category{
            CategoryId=2,
            Name="Bilgisayar"
           });
           _products.Add(new Product{
            ProductId=1,
            Name="Iphone 13",
            Price=40000,
            IsActive=false,
            Image="1.jpg",
            CategoryId=1,
           });
           _products.Add(new Product{
            ProductId=2,
            Name="Iphone 13 Pro",
            Price=50000,
            IsActive=true,
            Image="2.jpg",
            CategoryId=1,
           });
           _products.Add(new Product{
            ProductId=3,
            Name="Iphone 14",
            Price=60000,
            IsActive=false,
            Image="3.jpg",
            CategoryId=1,
           });
           _products.Add(new Product{
            ProductId=4,
            Name="Iphone 14 Pro",
            Price=70000,
            IsActive=true,
            Image="4.jpg",
            CategoryId=1,
           });
           _products.Add(new Product{
            ProductId=5,
            Name="MacBook Air",
            Price=70000,
            IsActive=true,
            Image="macbook-air.jpg",
            CategoryId=2,
           });
           _products.Add(new Product{
            ProductId=6,
            Name="MacBook Pro",
            Price=80000,
            IsActive=true,
            Image="macbook-pro.jpg",
            CategoryId=2,
           });
        }
        public static List<Product> Products{
            get{return _products;}
        }

        public static List<Category> Categories{
            get{return _categories;}
        }
        public static void CreateProduct(Product entity){
            _products.Add(entity);
        }
        public static void EditProduct(Product updatedProduct){
            var entity=_products.FirstOrDefault(x=>x.ProductId==updatedProduct.ProductId);
            if(entity!=null){
                entity.Name=updatedProduct.Name;
                entity.Price=updatedProduct.Price;
                entity.Image=updatedProduct.Image;
                entity.CategoryId=updatedProduct.CategoryId;
                entity.IsActive=updatedProduct.IsActive;
            }
        }
        public static void DeleteProduct(Product entiity){
            var removeEntity=_products.FirstOrDefault(x=>x.ProductId==entiity.ProductId);
           if(entiity!=null)
            _products.Remove(removeEntity);
        }

    }
}