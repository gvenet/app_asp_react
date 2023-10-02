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
          int randomIndex = random.Next(0, alphabet.Length);
          var category = categories.FirstOrDefault(c => c.Label == alphabet[randomIndex].ToString());

          if (category == null) {
            Console.WriteLine($"Category is null for Product {i}, Index : {randomIndex}");
          }

          var product = new Product {
            Label = $"Product {i}",
            Price = (float)random.NextDouble() * 100,
            Description = $"Description for Product {i}",
            Image_Url = $"ImageUrl{i}",
            Category = category!
          };
          products.Add(product);
        }
        context.Products.AddRange(products);
        context.SaveChanges();
      }
    }
  }
}
