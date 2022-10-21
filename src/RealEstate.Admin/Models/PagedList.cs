using System.Text.Json.Serialization;

namespace RealEstate.Admin.Models;

public class PagedList
{
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }
}
    
public class PagedList<T> : PagedList
{
    public List<T> List { get; set; }
}