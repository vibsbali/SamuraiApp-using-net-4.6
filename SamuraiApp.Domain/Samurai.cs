using System.Collections.Generic;

namespace SamuraiApp.Domain
{
   public class Samurai
   {
      public int Id { get; set; }
      public string Name { get; set; }

      //Foreign Id
      public int BattleId { get; set; }
      public List<Quote> Quotes { get; set; } = new List<Quote>();
   }
}
