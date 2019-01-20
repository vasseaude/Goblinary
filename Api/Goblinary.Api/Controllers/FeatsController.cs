using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Goblinary.Data.SqlServer;
using Goblinary.Model;

namespace Goblinary.Api.Controllers
{
    public class FeatsController : ApiController
    {
        private SqlContext db = new SqlContext();

        // GET: api/Feats
		/// <summary>
		/// Gets all Feats
		/// </summary>
		/// <param name="roleName">If present, controls which Role's Feats are returned.</param>
		/// <returns>A queryable list of Feats</returns>
        public IQueryable<Feat> GetFeats(string roleName = null)
        {
            return db.Feats.Where(x => x.Role.Name == (roleName ?? x.Role.Name));
        }

        // GET: api/Feats/5
        [ResponseType(typeof(Feat))]
        public IHttpActionResult GetFeat(string id)
        {
            Feat feat = db.Feats.Find(id);
            if (feat == null)
            {
                return NotFound();
            }

            return Ok(feat);
        }

        // PUT: api/Feats/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeat(string id, Feat feat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feat.Name)
            {
                return BadRequest();
            }

            db.Entry(feat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Feats
        [ResponseType(typeof(Feat))]
        public IHttpActionResult PostFeat(Feat feat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Feats.Add(feat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FeatExists(feat.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = feat.Name }, feat);
        }

        // DELETE: api/Feats/5
        [ResponseType(typeof(Feat))]
        public IHttpActionResult DeleteFeat(string id)
        {
            Feat feat = db.Feats.Find(id);
            if (feat == null)
            {
                return NotFound();
            }

            db.Feats.Remove(feat);
            db.SaveChanges();

            return Ok(feat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatExists(string id)
        {
            return db.Feats.Count(e => e.Name == id) > 0;
        }
    }
}