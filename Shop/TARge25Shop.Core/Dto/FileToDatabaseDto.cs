using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Dto
{
    public class FileToDatabaseDto
    {
        public Guid Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? KindergardenId { get; set; }
    }
}
