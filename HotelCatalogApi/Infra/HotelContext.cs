using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelCatalogApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace HotelCatalogApi.Infra
{
    public class HotelContext
    {
        private IConfiguration configuration;
        private IMongoDatabase database;
        //List<Hotel> hotelist = new List<Hotel>()
        //{
        //    new Hotel{Id="1",HotelDescription="Good location",HotelName="Vivanta",HotelStars=HotelStars.FiveStarHotel,City="Pune",HotelAvailability="Yes",HotelLocationPath="Hinjewadi"},
        //    new Hotel{Id="2",HotelDescription="Good Area",HotelName="Taj",HotelStars=HotelStars.FiveStarHotel,City="Pune",HotelAvailability="Yes",HotelLocationPath="Aundh"},
        //    new Hotel{Id="3",HotelDescription="Nice infrastcuture",HotelName="DIva",HotelStars=HotelStars.FiveStarHotel,City="Pune",HotelAvailability="Yes",HotelLocationPath="Wakad"},
        //    new Hotel{Id="4",HotelDescription="Good Welcome",HotelName="Manhattan",HotelStars=HotelStars.FiveStarHotel,City="Pune",HotelAvailability="Yes",HotelLocationPath="Shivaji nagar"}
        //};
        public HotelContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = configuration.GetValue<string>("MongoSettings:ConnectionString");

            // MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            MongoClient client = new MongoClient(settings);
            if (client != null)
            {
                this.database = client.GetDatabase(configuration.GetValue<string>("MongoSettings:Database"));
            }
        }

        public IMongoCollection<Hotel> HotelsCatalog
        {
            get
            {
                return this.database.GetCollection<Hotel>("Hotels");
            }
        }
    }
}
