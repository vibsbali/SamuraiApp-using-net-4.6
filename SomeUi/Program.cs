using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUi
{
   class Program
   {
      private static SamuraiContext _context = new SamuraiContext();

      static void Main(string[] args)
      {
         //InsertSamurai();

         //InsertMultipleSamurai();

         //InsertMultipleDifferentObjects();

         //SimpleSamuraiQuery();

         //EfFunctionQueries();

         //InsertNewPkFkGraph();

         //InsertNewPkFkGraphMultipleChildren();

         //AddChildToExistingObjectWhileTracked();

         //AddChildToExistingObjectWhileNotTracked();

         //EagerLoadingSamuraiWithQuotes();

         ModifyingRelatedDataWhenNotTracked();
      }

      private static void ModifyingRelatedDataWhenNotTracked()
      {
         var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
         var quote = samurai.Quotes[0];
         quote.Text += " Did you hear that?";
         using (var ctx = new SamuraiContext())
         {
            ctx.Entry(quote).State = EntityState.Modified;
            ctx.SaveChanges();
         }
      }

      private static void EagerLoadingSamuraiWithQuotes()
      {
         var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
         foreach (var samurai in samuraiWithQuotes)
         {
            Console.WriteLine(samurai.Name);
            foreach (var quote in samurai.Quotes)
            {
               Console.WriteLine("----" + quote.Text);
            }
         }
      }

      private static void AddChildToExistingObjectWhileNotTracked()
      {
         var samurai = _context.Samurais.First();

         //All we need to do is set the foreign key
         var quote =new Quote
         {
            SamuraiId = samurai.Id,
            Text = "I bet you're happy that I've saved you!"
         };

         using (var ctx = new SamuraiContext())
         {
            ctx.Quotes.Add(quote);
            ctx.SaveChanges();
         }
         
      }

      private static void AddChildToExistingObjectWhileTracked()
      {
         var samurai = _context.Samurais.First();
         samurai.Quotes.Add(new Quote
         {
            Text = "I bet you're happy that I've saved you!"
         });
         _context.SaveChanges();
      }

      private static void InsertNewPkFkGraphMultipleChildren()
      {
         var samurai = new Samurai
         {
            Name = "Shimoni Ninja",
            Quotes = new List<Quote>
            {
               new Quote {Text = "I've come to get you"},
               new Quote {Text = "I am the honour"}
            }
         };

         _context.Samurais.Add(samurai);
         _context.SaveChanges();
      }

      private static void InsertNewPkFkGraph()
      {
         var samurai = new Samurai
         {
            Name = "Kambei Shimada",
            Quotes = new List<Quote>
            {
               new Quote {Text = "I've come to save you"}
            }
         };

         _context.Samurais.Add(samurai);
         _context.SaveChanges();
      }

      private static void EfFunctionQueries()
      {
         var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, string.Concat("o", "%"))).ToList();
         foreach (var samurai in samurais)
         {
            Console.WriteLine(samurai.Name);
         }
      }

      private static void SimpleSamuraiQuery()
      {
         using (var ctx = new SamuraiContext())
         {
            var samurais = ctx.Samurais.ToList();

            foreach (var samurai in samurais)
            {
               Console.WriteLine(samurai.Name);
            }
         }
      }

      private static void InsertMultipleDifferentObjects()
      {
         var samurai = new Samurai {Name = "Osaka"};
         var battle = new Battle
         {
            Name = "Battle of Nagashino",
            StartDate = new DateTime(day:16, month:06, year:1575),
            EndDate = new DateTime(day:28, month:06, year:1575)
         };

         using (var ctx = new SamuraiContext())
         {
            ctx.AddRange(samurai, battle);
            ctx.SaveChanges();
         }
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
