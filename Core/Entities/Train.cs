using System;

namespace Core.Entities
{
    public class Train
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime AriveTime { get; set; }

        public int ClassAPrice { get; set; }

        public int ClassBPrice { get; set; }

        public int ClassCPrice { get; set; }

    }

}