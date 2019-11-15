using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCatalogApi.Models
{
    public class Hotel
    {
        [BsonId(IdGenerator = typeof(BsonBinaryDataGuidGenerator))]
        public Guid Id { get; set; }
        [BsonElement("hotelName")]
        public string HotelName { get; set; }
        [BsonElement("hotelStars")]
        public HotelStars HotelStars { get; set; }

        // Link to hotel rooms collection
      //  [BsonElement("rooms")]
       // public virtual List<Room> Rooms { get; set; }

        [BsonElement("hotelImageUrl")]
        public string HotelImageUrl { get; set; }

        [BsonElement("hotelLocationPath")]
        public string HotelLocationPath { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("hotelDescription")]
        public string HotelDescription{ get; set; }
        [BsonElement("hotelAvailability")]
        public string HotelAvailability { get; set; }
    }
}
