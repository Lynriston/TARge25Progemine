using System;
using System.Collections.Generic;
using System.Text;

namespace TARge25Shop.Core.Domain
{
    public class FileToApi
    {
        public Guid Id { get; set; }
        public string? ExistingFilePath { get; set;  }
        public Guid? SpaceshipId { get; set; }
    }
}
