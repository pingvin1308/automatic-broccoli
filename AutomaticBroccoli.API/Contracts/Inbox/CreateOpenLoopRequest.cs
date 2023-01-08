using System.ComponentModel.DataAnnotations;

namespace AutomaticBroccoli.API.Contracts;

/// <summary>
/// Request body for creating new open loop
/// </summary>
public sealed class CreateOpenLoopRequest
{
    /// <summary>
    /// Open loop's note.
    /// Attachments, links are optional.
    /// </summary>
    [Required]
    [StringLength(Domain.Note.MaxNoteLength)]
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// Open loop's attachments.
    /// </summary>
    public Guid[] Attachments { get; set; } = Array.Empty<Guid>();
}