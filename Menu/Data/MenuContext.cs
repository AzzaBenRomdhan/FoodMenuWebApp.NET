using Menu.Models;
using Microsoft.EntityFrameworkCore;

namespace Menu.Data
{
    public class MenuContext: DbContext
    {
        public MenuContext (DbContextOptions<MenuContext> options) : base (options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Définition de la clé primaire composite pour la table DishIngredient
            // Cela utilise deux clés : DishId et IngredientId pour identifier de manière unique chaque relation entre un plat et un ingrédient
            modelBuilder.Entity<DishIngredient>().HasKey(di => new
            {
                di.DishId,       // Clé composée : DishId
                di.IngredientId  // Clé composée : IngredientId
            });

            // Configuration de la relation entre DishIngredient et Dish
            // Chaque DishIngredient a une référence vers un Dish
            modelBuilder.Entity<DishIngredient>().HasOne(d => d.Dish)   // DishIngredient appartient à Dish
                .WithMany(di => di.DishIngredients)   // Un Dish peut avoir plusieurs DishIngredients
                .HasForeignKey(d => d.DishId);       // La clé étrangère est DishId dans DishIngredient

            // Configuration de la relation entre DishIngredient et Ingredient
            // Chaque DishIngredient a une référence vers un Ingredi
            modelBuilder.Entity<DishIngredient>().HasOne(i => i.Ingredient)  // DishIngredient appartient à Ingredient
                .WithMany(di => di.DishIngredients)   // Un Ingredient peut avoir plusieurs DishIngredients
                .HasForeignKey(i => i.IngredientId);  // La clé étrangère est IngredientId dans DishIngredient

            // Initialisation de données pour Dish (plat)
            // On ajoute un plat 'Margheritta' avec l'ID 1, le prix de 7.50 et une URL d'image
            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    Id = 1,
                    Name = "Margheritta",
                    Price = 7.50,
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0205/9582/articles/20220211142347-margherita-9920_ba86be55-674e-4f35-8094-2067ab41a671.jpg?crop=center&height=915&v=1644590192&width=1200"
                }
            );


            // Initialisation de données pour Ingredient (ingrédient)
            // Ajout de deux ingrédients : "Tomato Sauce" et "Mozzarella"
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Tomato Sauce" },
                new Ingredient { Id = 2, Name = "Mozzarella" }
            );

            // Initialisation de données pour DishIngredient (relation entre Dish et Ingredient)
            // Lien entre 'Margheritta' (DishId = 1) et les deux ingrédients (Tomato Sauce et Mozzarella)
            modelBuilder.Entity<DishIngredient>().HasData(
                new DishIngredient { DishId = 1, IngredientId = 1 },  // Lien entre 'Margheritta' et 'Tomato Sauce'
                new DishIngredient { DishId = 1, IngredientId = 2 }   // Lien entre 'Margheritta' et 'Mozzarella'
            );

            // Appel de la méthode de base pour assurer le bon comportement du framework
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
    }
}
