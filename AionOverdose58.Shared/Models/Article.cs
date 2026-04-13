namespace AionOverdose58.Shared.Models;

public class Article
{
    public int              Id             { get; set; }
    public ArticleCategory  Category       { get; set; } = ArticleCategory.Announcement;
    public string           Title          { get; set; } = string.Empty;
    public DateTime         CreatedOn      { get; set; } = DateTime.UtcNow;
    public DateTime         UpdatedOn      { get; set; } = DateTime.UtcNow;
    public DateTime?        PostedDate     { get; set; }
    public bool             IsDeleted      { get; set; } = false;
    public string           PreviewContent { get; set; } = string.Empty;
    public string           Content        { get; set; } = string.Empty;  // HTML
    public string?          ImageUrl       { get; set; }
}
