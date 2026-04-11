namespace AionOverdose58.Shared.DTOs;

public class NewsArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime PublishedAt { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}

public class NewsArticleDetailDto : NewsArticleDto
{
    public string Content { get; set; } = string.Empty;
}
