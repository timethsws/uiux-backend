using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class PlaceDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string MainImage { get; set; }

        public List<string> Images { get; set; }

        public bool Liked { get; set; }
    }
}
