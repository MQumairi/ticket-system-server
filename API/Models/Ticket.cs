using System;
using System.ComponentModel.DataAnnotations;

public class Ticket {

    [Key]
    public Guid ticketNumb {get; set;}

    [Required]
    public int authorId {get; set;}

    [Required]
    public string status {get; set;}

    [Required]
    public string product {get; set;}

    [Required]
    public string title {get; set;}

    [Required]
    public string date {get; set;}

    [Required]
    public string description {get; set;}

}