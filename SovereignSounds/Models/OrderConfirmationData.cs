using System.ComponentModel.DataAnnotations;

namespace SovereignSounds.Models;

public class OrderConfirmationData
{
    [Required(ErrorMessage = "Please enter your first name")]
    [Display(Name = "First Name")]
    [StringLength(30, ErrorMessage = "First Name cannot exceed 30 characters")]
    [RegularExpression(@"^[a-zA-Z '-]+$",
        ErrorMessage = "First Name can only contain letters, spaces, hyphens, and apostrophes")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your first name")]
    [Display(Name = "Last Name")]
    [StringLength(30, ErrorMessage = "Last Name cannot exceed 30 characters")]
    [RegularExpression(@"^[a-zA-Z '-]+$",
        ErrorMessage = "Last Name can only contain letters, spaces, hyphens, and apostrophes")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address")]
    [Display(Name = "Email Address")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Email Address must match: username@example.com, user.name@example.com, user.name+tag@example.com, john_doe@sub.domain.co")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number")]
    [Display(Name = "Phone Number")]
    [RegularExpression(@"^(\+1|1)?\s?\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$",
        ErrorMessage = "Phone Number must match: (XXX) XXX-XXXX, XXX-XXX-XXXX, XXXXXXXXXX")]
    public string Phone { get; set; } = string.Empty;



    [Required(ErrorMessage = "Please enter your shipping address")]
    [StringLength(100, ErrorMessage = "Shipping address cannot exceed 100 characters")]
    [Display(Name = "Shipping Address")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your credit card number")]
    [RegularExpression(@"^(?:\d{4}[- ]?){3}\d{4}$",
        ErrorMessage = "Credit Card Number must match: XXXXXXXXXXXXXXXX, XXXX XXXX XXXX XXXX, XXXX-XXXX-XXXX-XXXX")]
    [Display(Name = "Credit Card Number")]
    public string CreditCardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the cardholder name")]
    [StringLength(50, ErrorMessage = "Cardholder name cannot exceed 50 characters")]
    [Display(Name = "Card Name")]
    public string CreditCardName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the expiry date")]
    [RegularExpression(@"^(0[1-9]|1[0-2])/[0-9]{2}$", ErrorMessage = "Expiry date must be in MM/YY format")]
    [Display(Name = "Expiry Date")]
    public string ExpiryDate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the CVV")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be 3 digits")]
    public string CVV { get; set; } = string.Empty;
}
