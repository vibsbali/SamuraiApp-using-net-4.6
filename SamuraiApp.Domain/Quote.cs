namespace SamuraiApp.Domain
{
   public class Quote
   {
      public int Id { get; set; }
      public string Text { get; set; }

      //Navigation Property back to one Samurai
      public Samurai Samurai { get; set; }
      //Foreign Id
      public int SamuraiId { get; set; }
   }
}