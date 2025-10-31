namespace PersonMinimalApi.DTO;

public record PersonResponse(int Id, string Name, int? Age, string? Address, string? Work);