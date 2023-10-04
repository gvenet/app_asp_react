using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TodoApi.Models {
  public static class SeedData {
    public static void Initialize(IServiceProvider serviceProvider) {
      using (var context = new ApiContext(
          serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>())) {

        if (context.Products.Any()) {
          return;
        }

        string alphabet = "ABCDEFGHIJ";

        Random random = new Random();
        List<Category> categories = new List<Category>();
        for (int i = 0; i < 10; i++) {
          var category = new Category {
            Label = alphabet[i].ToString()
          };
          categories.Add(category);
          context.Categories.Add(category);
        }
        context.SaveChanges();

        var products = new List<Product>();
        for (int i = 1; i <= 100; i++) {
          var product = new Product {
            Label = $"Product {i}",
            Price = (float)random.NextDouble() * 100,
            Description = $"Description for Product {i}",
            Image_Url = $"ImageUrl{i}",
            Version = 1.0f
          };
          products.Add(product);
        }
        context.Products.AddRange(products);
        context.SaveChanges();

        var productsCategories = new List<ProductCategory>();

        for (int j = 0; j < 5; j++) {
          for (int i = 1; i <= 100; i++) {
            int randomIndex = random.Next(0, alphabet.Length);
            var product = products.FirstOrDefault(p => p.Label == $"Product {i}");
            var category = categories.FirstOrDefault(c => c.Label == alphabet[randomIndex].ToString());
            if (!productsCategories.Any(pc => pc.ProductId == product!.Id && pc.CategoryId == category!.Id)) {
              productsCategories.Add(new ProductCategory { ProductId = product!.Id, CategoryId = category!.Id });
            }
          }
        }
        context.ProductsCategories.AddRange(productsCategories);
        context.SaveChanges();
      }
    }
  }
}
