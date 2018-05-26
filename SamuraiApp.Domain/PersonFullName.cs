using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
   public class PersonFullName
   {
      public PersonFullName(string surName, string givenName)
      {
         SurName = surName;
         GivenName = givenName;
      }

      public string GivenName { get; set; }

      public string SurName { get; set; }

      public string FullName => $"{GivenName} {SurName}";
      public string FullNameReverse => $"{SurName} {GivenName}";

   }
}
