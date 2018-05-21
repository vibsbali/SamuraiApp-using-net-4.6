using System.Collections.Generic;

namespace SamuraiApp.Domain
{
   public class Samurai
   {
      public int Id { get; set; }
      public string Name { get; set; }

      //Foreign Id
      //public int BattleId { get; set; }
      
      //Setting up many to many relationship - this is called Join Entitiy
      public List<SamuraiBattle> SamuraiBattles { get; set; }
      public List<Quote> Quotes { get; set; } = new List<Quote>();
   }
}
