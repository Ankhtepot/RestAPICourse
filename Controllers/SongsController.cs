using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : Controller
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST api/<SongsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;

            var audioUrl = await FileHelper.UploadAudioFile(song.AudioFile);
            song.AudioUrl = audioUrl;

            song.UploadedDate = DateTime.Now;

            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        // api/songs
        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var songs = await (from song in _dbContext.Songs
                               select new 
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).ToListAsync();

            return Ok(songs.Paginate(currentPageNumber, currentPageSize));
        }

        // api/songs/featuredsongs
        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in _dbContext.Songs
                               where song.IsFeatured
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).ToListAsync();

            return Ok(songs);
        }

        // api/songs/newsongs
        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in _dbContext.Songs
                               orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).Take(10)
                               .ToListAsync();

            return Ok(songs);
        }

        // api/songs/newsongs
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in _dbContext.Songs
                               where song.Title.ToLower().StartsWith(query.ToLower())
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl
                               }).Take(10)
                               .ToListAsync();

            return Ok(songs);
        }
    }
}
