using Infinite.Core.Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Infinite.Core.Project.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IConfiguration _configuration;

        public MoviesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<MovieViewModel> movies = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Movies/GetAllMovies");
                if (result.IsSuccessStatusCode)
                {
                    movies = await result.Content.ReadAsAsync<List<MovieViewModel>>();
                }
            }
            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            MovieViewModel movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Movies/GetMovieById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    movie = await result.Content.ReadAsAsync<MovieViewModel>();
                }
            }
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            MovieViewModel viewModel = new MovieViewModel
            {
                Genres = await this.GetGenres()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Movies/CreateMovie", movie);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            MovieViewModel viewModel = new MovieViewModel
            {
                Genres = await this.GetGenres()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                MovieViewModel movie = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Movies/GetMovieById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        movie = await result.Content.ReadAsAsync<MovieViewModel>();
                        movie.Genres = await this.GetGenres();
                        return View(movie);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Movie doesn't exists");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Movies/UpdateMovie/{movie.Id}", movie);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
                    }
                }
            }
            MovieViewModel viewModel = new MovieViewModel
            {
                Genres = await this.GetGenres()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            MovieViewModel movie = await this.MovieById(id);
            if (movie != null)
            {
                return View(movie);
            }
            ModelState.AddModelError("", "Server Error. Please try later");
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MovieViewModel movieVM)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Movies/DeleteMovie/{movieVM.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }            
        }

        [HttpGet]
        [Route("Movies/Search/{genre?}")]
        public async Task<IActionResult> Search(string genre)
        {
            if (genre != null)
            {
                List<MovieViewModel> movie = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Movies/SearchMovie/{genre}");
                    if (result.IsSuccessStatusCode)
                    {
                        movie = await result.Content.ReadAsAsync<List<MovieViewModel>>();
                    }
                }
                return View(movie);
            }
            return View();
        }

        [NonAction]
        public async Task<List<GenreViewModel>> GetGenres()
        {
            List<GenreViewModel> genres = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Movies/GetGenres");
                if (result.IsSuccessStatusCode)
                {
                    genres = await result.Content.ReadAsAsync<List<GenreViewModel>>();
                }
            }
            return genres;
        }

        [NonAction]
        public async Task<MovieViewModel> MovieById(int id)
        {
            MovieViewModel movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Movies/GetMovieById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    movie = await result.Content.ReadAsAsync<MovieViewModel>();
                }
            }
            return movie;
        }
    }
}
