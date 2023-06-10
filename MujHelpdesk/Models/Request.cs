using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace MujHelpdesk.Models;

public partial class Request
{
    [Key]
    public int Id { get; set; }

	[Required(ErrorMessage = "Prosím vyplňte nadpis")]
	[StringLength(50)]
    public string Title { get; set; } = null!;

	[Required(ErrorMessage = "Prosím popište problém")]
	public string Text { get; set; } = null!;


	[StringLength(50)]
    public string Username { get; set; } = null!;

    public int Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrigDate { get; set; }

    [Column(TypeName = "image")]
	public byte[]? Img { get; set; }

	[InverseProperty("Req")]
    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
}
