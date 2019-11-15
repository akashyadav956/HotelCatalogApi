using HotelCatalogApi.Helpers;
using HotelCatalogApi.Infra;
using HotelCatalogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HotelCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class HotelCatalogController : ControllerBase
    {
        private HotelContext dbContext;
        IConfiguration config;
        public HotelCatalogController(HotelContext db, IConfiguration configuration)
        {
            this.dbContext = db;
            this.config = configuration;
        }

        [AllowAnonymous]
        [HttpGet("", Name = "GetHotels")]
        public async Task<ActionResult<List<Hotel>>> GetHotels()
        {
            var result = await this.dbContext.HotelsCatalog.FindAsync<Hotel>(FilterDefinition<Hotel>.Empty);
            return result.ToList();
        }

        [HttpPost("addHotel", Name = "AddHotel")]
        public ActionResult<Hotel> AddHotel()
        {
            var coverImageName = SaveImageToCloudAsync(Request.Form.Files[0]).GetAwaiter().GetResult();
            var HotelId = Request.Form["id"];
            HotelStars HotelStars;
            Enum.TryParse(Request.Form["hotelStars"], true, out HotelStars);
            var Hotel = new Hotel()
            {
                HotelName = Request.Form["hotelName"],
                HotelStars = HotelStars,
                // Rooms = Request.Form["rooms"],
                HotelImageUrl = coverImageName,
                HotelLocationPath = Request.Form["hotelLocationPath"],
                HotelDescription = Request.Form["hotelDescription"],
                HotelAvailability = Request.Form["hotelAvailability"]

            };

         
            dbContext.HotelsCatalog.InsertOne(Hotel);  // saving to mongo            
            return Hotel;
        }

        [HttpPost("updateHotel", Name = "UpdateHotel")]
        public ActionResult<Hotel> UpdateHotel()
        {
            var coverImageName = SaveImageToCloudAsync(Request.Form.Files[0]).GetAwaiter().GetResult();

            FilterDefinition<Hotel> filter = "{ Id:" + Request.Form["id"] + " }";

            //UpdateDefinition<BsonDocument> update = "{ $set: { x: 1 } }";

            var HotelId = Request.Form["id"];
            HotelStars HotelStars;
            Enum.TryParse(Request.Form["hotelStars"], true, out HotelStars);
            var Hotel = new Hotel()
            {
                HotelName = Request.Form["hotelName"],
                HotelStars = HotelStars,
                // Rooms = Request.Form["rooms"],
                //HotelImageUrl = coverImageName,
                HotelLocationPath = Request.Form["hotelLocationPath"],
                HotelDescription = Request.Form["hotelDescription"],
                HotelAvailability = Request.Form["hotelAvailability"]

            };
            //dbContext.HotelsCatalog.UpdateOne(filter, Hotel);  // saving to mongo     
            dbContext.HotelsCatalog.ReplaceOne(x => x.Id == HotelId, Hotel);
            return Hotel;
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetHotelById")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Hotel>> GetHotelById(Guid id)
        {

            var builder = Builders<Hotel>.Filter;

            var filter = builder.Eq("Id", id);

            var result = await dbContext.HotelsCatalog.FindAsync(filter);

            var item = result.FirstOrDefault();

            if (item == null)

            {

                return NotFound(); //Not found , Status code 404

            }

            else

            {

                return Ok(item); //Found , status code 200

            }

        }

        [AllowAnonymous]
        [HttpGet("{city}", Name = "GetHotelByCity")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<Hotel>>> GetHotelByCity(string city)
        {

            var builder = Builders<Hotel>.Filter;

            var filter = builder.Eq("City", city);

            var result = await dbContext.HotelsCatalog.FindAsync(filter);

            var item = result.ToList();

            if (item == null)

            {

                return NotFound(); //Not found , Status code 404

            }

            else

            {

                return Ok(item); //Found , status code 200

            }

        }

        [AllowAnonymous]
        [HttpDelete("DeleteHotel/{id}", Name = "DeleteById")]
        public string DeleteById(Guid id)

        {

            var result = this.dbContext.HotelsCatalog.DeleteOne(Hotel => Hotel.Id == id);

            if (result.DeletedCount > 0)

            {

                return "Deleted";

            }

            else

            {

                return "";

            }

        }

        [NonAction]
        private async Task<string> SaveImageToCloudAsync(IFormFile image)
        {
            var imageName = $"{Guid.NewGuid()}_{Request.Form.Files[0].FileName}";
            var tempFile = Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFile, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }

            var imageFile = Path.Combine(Path.GetDirectoryName(tempFile), imageName);
            System.IO.File.Move(tempFile, imageFile);
            StorageAccountHelper storageHelper = new StorageAccountHelper();
            storageHelper.StorageConnectionString = config.GetConnectionString("StorageConnection");
            var fileUri = await storageHelper.UploadFileToBlobAsync(imageFile, "eHotelsgalleryimages");

            System.IO.File.Delete(imageFile);

            //fileUri = fileUri + "?sv=2019-02-02&ss=bfq&srt=sco&sp=rwdlacup&se=2019-11-15T15:22:16Z&st=2019-11-06T07:22:16Z&spr=https&sig=Bshl%2Bplh44Wu3w4e8XIGemZgLhamR%2FrFXdDvIqJdkbg%3D";
            return fileUri;
        }
    }
}