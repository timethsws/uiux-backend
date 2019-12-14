using System;
namespace Core.Entities
{
    public class FavouritePlace
    {
        public Guid Id { get; set; }

        public Place Place { get; set; }
        public Guid PlaceId { get; set; }

        public ApplicationUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
