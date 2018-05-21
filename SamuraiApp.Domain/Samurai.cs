using System.Collections.Generic;
using System.Security.AccessControl;

namespace SamuraiApp.Domain
{
   public class Samurai
   {
      public int Id { get; set; }
      public string Name { get; set; }

      //Foreign Id
      //public int BattleId { get; set; }
      
      //Setting up many to many relationship for multiple battles for each Samurai
      //- this is called Join Entitiy
      public List<SamuraiBattle> SamuraiBattles { get; set; }

      //Setting up one to one - each samurai can have one and only one identity
      public SecretIdentity SecretIdentity { get; set; }
      public List<Quote> Quotes { get; set; } = new List<Quote>();
   }
}
