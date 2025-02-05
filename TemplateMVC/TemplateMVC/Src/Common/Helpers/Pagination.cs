using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateMVC.Common.Exceptions;

namespace TemplateMVC.Helpers;

public class Pagination<T>
{
    private readonly IHttpContextAccessor _contextAccessor;
    public IEnumerable<T> Items { get; private set; }
    public PaginationMetadata Metadata { get; set; }
    
    public Pagination(IEnumerable<T> items, int count, int pageIndex, int pageSize, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
       
        if (pageIndex < 0)
        {
            throw new ArgumentException("Invalid pageIndex: must be >= 0.");
        }
        if (pageSize < 1)
        {
            throw new ArgumentException("Invalid pageSize: must be >= 1.");
        }
        Items = items;
        Metadata = new PaginationMetadata
        {
            PageIndex = pageIndex,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
        CalculateUrls(pageIndex, pageSize);
    }

    private void CalculateUrls(int pageIndex, int pageSize)
    {
        var httpContext = _contextAccessor?.HttpContext;
        Metadata.FirstPageUrl = GetPageUrl(httpContext, pageSize, 0);
        if (pageIndex < Metadata.TotalPages)
        {
            Metadata.NextPageUrl = GetPageUrl(httpContext, pageSize, pageIndex + 1);
        }
        if (pageIndex > 0)
        {
            Metadata.PreviousPageUrl = GetPageUrl(httpContext, pageSize, pageIndex - 1);
        }
        Metadata.LastPageUrl = GetPageUrl(httpContext, pageSize, Metadata.TotalPages);
    }

    private string GetPageUrl(HttpContext? httpContext, int pageSize, int pageNumber)
    {
        var request = httpContext?.Request;
        var url = $"{request?.Scheme}://{request?.Host}{request?.PathBase}{request?.Path}?pageIndex={pageNumber}&pageSize={pageSize}";
        return url;
    }

    public bool HasPreviousPage()
    {
        return Metadata.PageIndex > 1;
    }

    public bool HasNextPage()
    {
        return Metadata.PageIndex < Metadata.TotalPages;
    }
}

public class PaginationMetadata
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public string? FirstPageUrl { get; set; }
    public string? LastPageUrl { get; set; }
    public string? PreviousPageUrl { get; set; }
    public string? NextPageUrl { get; set; }
}


public class PaginationParam
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set;} = 10;
}