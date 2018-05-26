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
         #region CourseOne

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

         //ModifyingRelatedDataWhenNotTracked();

         #endregion

         //PrePopulateSamuraisAndBattles();

         #region CourseTwo

         //JoinBattleAndSamurai();
         
         //EnlistSamuraiIntoBattleWhenTracked(_context.Battles.Find(1));
         
         //EnlistSamuraiIntoBattleWhenUnTracked(_context.Battles.Find(1));

         // GetSamuraiWithBattles();

         //AddNewSamuraiWithSecretIdentity();

         //AddSecretIdentityUsingSamuraiId();

         //AddSecretIdenityToExistingSamurai();

         //AddShadowProperties();

         //RetrieveUsingShadowProperties();

         AddNewSamuraiWithSecretIdentity();

         #endregion

      }

      private static void RetrieveUsingShadowProperties()
      {
         var oneWeekAgo = DateTime.Now.AddDays(-7);
         var newSamurais = _context.Samurais.Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
            .Select(s => new { s.Id, s.Name, Created = EF.Property<DateTime>(s, "Created")}).ToList();

         foreach (var newSamurai in newSamurais)
         {
            Console.WriteLine($"{newSamurai.Name}   {newSamurai.Created}" );
         }

      }

      private static void AddShadowProperties()
      {
         var samurai = new Samurai {Name = "Ronin"};
         _context.Samurais.Add(samurai);
         var timestamp = DateTime.Now;
         _context.Entry(samurai).Property("Created").CurrentValue = timestamp;
         _context.Entry(samurai).Property("LastModified").CurrentValue = timestamp;
         _context.SaveChanges();

      }

      private static void AddSecretIdenityToExistingSamurai()
      {
         Samurai samurai;
         using (var seperateOperation = new SamuraiContext())
         {
            samurai = seperateOperation.Samurais.Find(3);
         }

         samurai.SecretIdentity = new SecretIdentity {RealName = "Shamido"};
         //Here we have to use attach and what EF core will do is check if Samurai has an Id and if so it knows that the Samurai exists
         //And only add the new identity
         _context.Samurais.Attach(samurai);
         _context.SaveChanges();
      }

      private static void AddSecretIdentityUsingSamuraiId()
      {
         var identity = new SecretIdentity {SamuraiId = 2};
         //In EF Core we can work with context directly when db set is not available
         _context.Add(identity);
         _context.SaveChanges();
      }

      private static void AddNewSamuraiWithSecretIdentity()
      {
         var samurai = new Samurai {Name = "Jina Ujichika"};
         samurai.SecretIdentity = new SecretIdentity {RealName = "Julie"};
         _context.Samurais.Add(samurai);
         _context.SaveChanges();
      }

      private static void GetSamuraiWithBattles()
      {
         var samuraiWithBattles = _context.Samurais
            .Include(s => s.SamuraiBattles)
            .ThenInclude(sb => sb.Battle)
            .FirstOrDefault(s => s.Id == 1); //Notice that we used Include and ThenInclude before FirstOrDefault

         foreach (var samuraiWithBattle in samuraiWithBattles.SamuraiBattles)
         {
            Console.WriteLine(samuraiWithBattle.Battle.Name);
         }

      }

      private static void EnlistSamuraiIntoBattleWhenUnTracked(Battle battle)
      {
         battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 2});
         using (var ctx = new SamuraiContext())
         {
            ctx.Battles.Attach(battle);
            ctx.ChangeTracker.DetectChanges();
            ctx.SaveChanges();
         }
      }

      private static void EnlistSamuraiIntoBattleWhenTracked(Battle battle)
      {
         battle.SamuraiBattles.Add(new SamuraiBattle{ SamuraiId = 3});
         _context.SaveChanges();
      }

      private static void JoinBattleAndSamurai()
      {
         //Here we are creating a new samurai battle asserting that in battle 3 Samurai With Id 1 fought 
         var sbJoin = new SamuraiBattle {SamuraiId = 1, BattleId = 3};

         //IMPORTANT: Here we did not add an item to dbset we have no DbSet for SamuraiBattle and we are still adding a new item
         //into the database
         _context.Add(sbJoin);
         _context.SaveChanges();
      }

      private static void PrePopulateSamuraisAndBattles()
      {
         _context.AddRange(
            new Samurai { Name = "Kikuchiyo" },
            new Samurai { Name = "Kambei Shimada" },
            new Samurai { Name = "Shichirōji " },
            new Samurai { Name = "Katsushirō Okamoto" },
            new Samurai { Name = "Heihachi Hayashida" },
            new Samurai { Name = "Kyūzō" },
            new Samurai { Name = "Gorōbei Katayama" }
         );

         _context.Battles.AddRange(
            new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
            new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
            new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
            new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
         );
         _context.SaveChanges();
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
