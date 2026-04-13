using System.ComponentModel;

namespace AionOverdose58.Shared.Models;

public enum ArticleCategory
{
    Announcement = 1,
    Sales        = 2,
    [Description("Patch Notes")]
    PatchNotes   = 3,
    Event        = 4
}
