using BookStore.Constants;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookListGeneratorService bookListGeneratorService;
        private readonly ISaveFileToDataBaseService saveFileToDataBaseService;
        private readonly IBookSerachService bookSerachService;
        private readonly IBookManageFavsService bookManageFavsService;

        public HomeController(ILogger<HomeController> logger, 
                            IBookListGeneratorService bookListGeneratorService,
                            ISaveFileToDataBaseService saveFileToDataBaseService,
                            IBookSerachService bookSerachService,
                            IBookManageFavsService bookManageFavsService)
        {
            _logger = logger;
            this.bookListGeneratorService = bookListGeneratorService;
            this.saveFileToDataBaseService = saveFileToDataBaseService;
            this.bookSerachService = bookSerachService;
            this.bookManageFavsService = bookManageFavsService;
        }


        public IActionResult Index()
        {
            var books = this.bookSerachService.Process();
            if (books is null || !books.Any())
            {
                return RedirectToAction(nameof(UploadFile));
            }
            return View(books);
        }
        public IActionResult ShowFavorites ( )
        {
            var books = this.bookSerachService.GetFavouritesBook();
            return View("Index",books);
        }
        public IActionResult OrderBy(int orderBy)
        {
            var books = this.bookSerachService.GetSorteredBooks((EnumOrderBy)orderBy);
            return View("Index", books);
        }
        public async Task<IActionResult> Favourites(int Id)
        {
            await this.bookManageFavsService.Change(Id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Search (string search)
        {
            var books = this.bookSerachService.GetBooks(search);
            return View("Index", books);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {

            if (file != null && file.Length > 0)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                    {
                        string fileContent = reader.ReadToEnd();
                        var bookList = this.bookListGeneratorService.Process(fileContent);
                        if (bookList is not null)
                        {
                            await this.saveFileToDataBaseService.Proccess(bookList);
                        }
                        ViewBag.Message = "File uploaded successfully!";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "An error occurred: " + ex.Message;
                    return View("Error");
                }
            }
            else
            {
                ViewBag.Error = "Please select a file.";
                return View("UploadFile");
            }

            return RedirectToAction(nameof(Index)); 
        }

    }
}