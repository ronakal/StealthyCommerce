using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StealthyCommerce.Service.ManageOffer;

namespace StealthyCommerce.WebAPI.Controllers
{
    /// <summary>
    /// Everything offers, offers all of the basic CRUD operations for offers. Intended for administrative view use.
    /// </summary>
    [Route("api/[controller]")]
    public class OfferController : Controller
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        /// <summary>
        /// See all of existing offers. This is the recommended API point for managing offers in the database. If you are 
        /// looking for something customer facing, I would recommend seeing the ProductOffer API.
        /// </summary>
        /// <param name="pageNumber">zero based number for page</param>
        /// <param name="pageSize">the number of records to include</param>
        /// <returns>All offers in the system requested</returns>
        // GET: api/<controller>
        [HttpGet]
        public IQueryable<OfferDTO> Get(int pageNumber = 0, int pageSize = 10)
        {
            return this.offerService.GetOffers().OrderByDescending(o => o.DateModified).Skip((pageNumber + 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Retrieves the specific offer for editing/deleting/viewing details.
        /// </summary>
        /// <param name="id">the id of the offer</param>
        /// <returns>the full offer details</returns>
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public OfferDTO Get(int id)
        {
            return this.offerService.GetOffers().Where(o => o.OfferId == id).FirstOrDefault();
        }

        /// <summary>
        /// Will create a new instance of the offer provided.
        /// </summary>
        /// <param name="offer">the offer to create</param>
        /// <returns>the id of the offer just created</returns>
        // POST api/<controller>
        [HttpPost]
        public int Post([FromBody]OfferDTO offer)
        {
            return this.offerService.CreateOffer(offer);
        }

        /// <summary>
        /// The offer to update, you must provide the id as it will override the offer's id property.
        /// </summary>
        /// <param name="id">the id of the offer</param>
        /// <param name="offer">all properties excluding (id, create date, modified date) are editable</param>
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]OfferDTO offer)
        {
            offer.OfferId = id;
            this.offerService.UpdateOffer(offer);
        }

        /// <summary>
        /// The offer to delete, this is final, I hope your application asks the "Are you sure?" question.
        /// </summary>
        /// <param name="id">the id of the offer to delete</param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.offerService.DeleteOffer(id);
        }
    }
}
