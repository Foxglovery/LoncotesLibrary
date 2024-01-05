using System.ComponentModel.DataAnnotations;


namespace LoncotesLibrary.Models;

public class Checkout
{
    public int Id { get; set; }
   
    public int MaterialId { get; set; }
    public Material Material { get; set; }
    
    public int PatronId { get; set; }
    public Patron Patron { get; set; }
    [Required]
    public DateTime CheckoutDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; } = null;
    public bool Paid { get; set; }

 
}