namespace PetLyve.Application.Pagination;

/// <summary>
/// Lançada quando page ou pageSize estão fora do intervalo permitido.
/// Herda de <see cref="ArgumentException"/> para continuar caindo no 400
/// da tabela de mapeamento do GlobalExceptionHandler (CP3).
/// </summary>
public class InvalidPaginationException : ArgumentException
{
    public InvalidPaginationException(IReadOnlyDictionary<string, string[]> errors)
        : base(string.Join(" ", errors.Values.SelectMany(messages => messages)))
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}
