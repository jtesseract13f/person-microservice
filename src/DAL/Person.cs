using System.ComponentModel.DataAnnotations;

namespace PersonMinimalApi;

public class Person
{
    [Key]
    public int Id { get; set; }
    [MaxLength(256)]
    public string? Name { get; set; }
    public int? Age { get; set; }
    [MaxLength(256)]
    public string? Address { get; set; }
    [MaxLength(256)]
    public string? Work {get; set;}
}