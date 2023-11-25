using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Index(string searchString,string category)
    {
        ViewBag.searchString=searchString;
        var products=Repository.Products;
        if(!string.IsNullOrEmpty(searchString)){
            products=products.Where(p=>p.Name!.ToLower().Contains(searchString)).ToList();
        }
        if(!string.IsNullOrEmpty(category)&& category!="0"){
            products=products.Where(x=>x.CategoryId==int.Parse(category)).ToList();
        }
        // ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name",category);
        var model=new ProductViewModel(){
            Products=products,
            Categories=Repository.Categories,
            SelectedCategory=category
            
        };
        return View(model);
    }
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult>  Create(Product model,IFormFile imagefile){
       var allowedExtensions=new[]{".jpg",".jpeg",".png"};
       var extension=Path.GetExtension(imagefile.FileName);
       var randomFileName=string.Format($"{Guid.NewGuid().ToString()}{extension}");
        var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",imagefile.FileName);
       if(imagefile!=null){
        if(!allowedExtensions.Contains(extension)){
            ModelState.AddModelError("","Geçerli bir resim seçiniz...");
        }
       }
       if(ModelState.IsValid){
        using(var stream=new FileStream(path,FileMode.Create)){
            await imagefile!.CopyToAsync(stream);
        }
        model.Image=randomFileName;
            model.ProductId=Repository.Products.Count()+1;
        Repository.CreateProduct(model);
        return RedirectToAction("Index");
        }
        ViewBag.categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
       
    }
    [HttpGet]
    public IActionResult Edit(int? id){
        if(id==null){
            return NotFound();
        }
        var entity=Repository.Products.FirstOrDefault(x=>x.ProductId==id);
        if(entity==null){
            return NotFound();
        }
        ViewBag.categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(entity);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id,Product model,IFormFile? imagefile){
       var extension="";
        if(id!=model.ProductId){
            return NotFound();
        }
        
        if(ModelState.IsValid){
           
            if(imagefile!=null){
                  var allowedExtensions=new[]{".jpg",".jpeg",".png"};
        extension=Path.GetExtension(imagefile.FileName);
       var randomFileName=string.Format($"{Guid.NewGuid().ToString()}{extension}");
        var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",imagefile.FileName);
                using(var stream=new FileStream(path,FileMode.Create)){
            await imagefile!.CopyToAsync(stream);
            model.Image=randomFileName;
        }
        }
        else{
            model.Image=Repository.Products.FirstOrDefault(y=>y.ProductId==id).Image;
        }
        
            
            Repository.EditProduct(model);
            return RedirectToAction("Index");
        }

        ViewBag.categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
        
    }
    [HttpGet]
    public IActionResult Delete(int? id){
        if(id==null)
        return NotFound();
        else{
            var entity=Repository.Products.FirstOrDefault(x=>x.ProductId==id);
            if(entity==null){
            return NotFound();
            }
            
            return View("DeleteConfirm",entity);
        }
    }
    [HttpPost]
    public IActionResult Delete(int ProductId) {
       var entity=Repository.Products.FirstOrDefault(x=>x.ProductId==ProductId);
       Repository.DeleteProduct(entity);
       return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult EditProducts(List<Product> products){
        foreach(var item in products){
            Repository.EditProduct(item);
        }
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
