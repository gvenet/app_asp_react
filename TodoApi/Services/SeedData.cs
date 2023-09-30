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

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        Random random = new Random();
        
        for (int i = 0; i < 100; i++) {
          char selectedLetter1 = alphabet[random.Next(alphabet.Length)];
          string selectedLabel = $"Produit {selectedLetter1}";
          
          char selectedLetter2 = alphabet[random.Next(alphabet.Length)];
          string category = $"Catégorie {selectedLetter2}";

          var product = new Product {
            Label = selectedLabel,
            Price = (float)Math.Round(random.NextDouble() * (100 - 1) + 1,2),
            Description = $"Description du {selectedLabel}",
            Image_Url = $"url_image_{i + 1}",
            Version = (float)Math.Round(random.NextDouble() * (5 - 1) + 1,2),
            Category = category
          };

          context.Products.Add(product);
        }

        context.SaveChanges();
      }
    }
  }
}
