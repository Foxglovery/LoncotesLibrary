

using System.Xml.Schema;

namespace LoncotesLibrary.Models.DTOs;

public class PatronDTO
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }


    public string Address { get; set; }


    public string Email { get; set; }

    public bool IsActive { get; set; }
    //IF THIS BREAKS CHANGE CHECKOUTWITHLATEFEEDTO BACKTO CHECKOUTDTO
    public List<CheckoutWithLateFeeDTO> Checkouts { get; set; }

}