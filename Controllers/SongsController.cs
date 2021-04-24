using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // GET: api/<SongsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //return _dbContext.Songs;
            //return BadRequest();
            //return NotFound();
            //return StatusCode(200);
            //return StatusCode(StatusCodes.Status200OK);

            return Ok(await _dbContext.Songs.ToListAsync());
        }

        // GET api/<SongsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var song = await _dbContext.Songs.FindAsync(id);

            if (song == null)
            {
                //return StatusCode(StatusCodes.Status404NotFound);
                return NotFound("Not record found against this Id.");
            }

            return Ok(song);
        }

        // api/songs/test/1
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }

        //// POST api/<SongsController>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Song song)
        //{
        //    await _dbContext.Songs.AddAsync(song);
        //    await _dbContext.SaveChangesAsync();

        //    return StatusCode(StatusCodes.Status201Created);
        //}

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;

            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Song songObj)
        {
            var song = await _dbContext.Songs.FindAsync(id);

            if (song == null)
            {
                //return StatusCode(StatusCodes.Status404NotFound);
                return NotFound("Not record found against this Id.");
            }

            song.Title = songObj.Title;
            song.Language = songObj.Language;
            song.Duration = songObj.Duration;
            await _dbContext.SaveChangesAsync();

            return Ok("Record updated successfully.");
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _dbContext.Songs.FindAsync(id);

            if (song == null)
            {
                //return StatusCode(StatusCodes.Status404NotFound);
                return NotFound("Not record found against this Id.");
            }

            _dbContext.Songs.Remove(song);
            await _dbContext.SaveChangesAsync();

            return Ok("Record Deleted.");
        }
    }
}
