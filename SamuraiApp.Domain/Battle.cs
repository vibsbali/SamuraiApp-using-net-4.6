using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace SamuraiApp.Domain
{
   public class Battle
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      //public List<Samurai> Samurais { get; set; }

      //Setting up many to many relationship One Samurai can fight in multiple battles and 
      //one battle can have multiple Samurais
      public List<SamuraiBattle> SamuraiBattles { get; set; } = new List<SamuraiBattle>();
   }
}