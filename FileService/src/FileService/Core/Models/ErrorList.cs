using System.Collections;

namespace FileService.Core.Models;

public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;
    
    public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();

    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = errors.ToList();
    }
    
    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(List<Error> errors)
        => new(errors);

    public static implicit operator ErrorList(Error error)
        => new([error]);
}