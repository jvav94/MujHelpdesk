using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MujHelpdesk.Models;

public partial class Comment
{
    [Key]
    public int Id { get; set; }

    public int ReqId { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    public string Text { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[]? Img { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PostDate { get; set; }

    [ForeignKey("ReqId")]
    [InverseProperty("Comments")]
    public virtual Request Req { get; set; } = null!;
}
