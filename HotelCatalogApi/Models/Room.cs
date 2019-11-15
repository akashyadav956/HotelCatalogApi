using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCatalogApi.Models
{
    public enum RoomType { SingleRoom, DoubleRoom, DeluxeRoom, DuplexRoom, Studio, Penthouse };
    public class Room
    {
        public string Id { get; set; }
        public string HotelId { get; set; }
        public int RoomPrice { get; set; }
        public RoomType RoomType { get; set; }
        // Link to the hotel where room is
        public virtual Hotel Hotel { get; set; }
    }
}
