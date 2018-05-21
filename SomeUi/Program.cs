using System;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUi
{
   class Program
   {
      static void Main(string[] args)
      {
         InsertSamurai();

         InsertMultipleSamurai();
         
      }

      private static void InsertMultipleSamurai()
      {
         var samurai = new Samurai
         {
            Name = "Niko"
         };

         var samuraiDog = new Samurai
         {
            Name = "Alice"
         };

         using (var context = new SamuraiContext())
         {
            context.Samurais.AddRange(samurai, samuraiDog);
            context.SaveChanges();
         }
      }

      private static void InsertSamurai()
      {
         var samurai = new Samurai
         {
            Name = "Mishu"
         };

         using (var context = new SamuraiContext())
         {
            context.Samurais.Add(samurai);
            context.SaveChanges();
         } 
      }
   }
}
