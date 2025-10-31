namespace PersonMinimalApi.DTO;

public record PersonRequest (string Name, int? Age, string? Address, string? Work);
public record PersonPatchRequest(string? Name, int? Age, string? Address, string? Work);