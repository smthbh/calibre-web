/*
 * CalibreWeb
 * 
 * Copyright (C) 2018..2021 by Simon Baer
 *
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 * 
 */

using Microsoft.AspNetCore.Mvc;

namespace CalibreWeb.Controllers
{
    public class CoverController : Controller
    {
        private IConfiguration configuration;
        private readonly ILogger<CoverController> _logger;

        public CoverController(IConfiguration configuration, ILogger<CoverController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        [ResponseCache(Duration = 86400)]
        public ActionResult Index(string path)
        {
            path = System.Text.Encoding.Default.GetString(Convert.FromBase64String(path));
            path = Path.GetFullPath(Path.Combine(configuration["Calibre:CataloguePath"], path));
            _logger.LogInformation($"Request for cover full path: {path}", path);

            string file = Path.Combine(path, "cover.jpg");
            if (System.IO.File.Exists(file) && path.StartsWith(configuration["Calibre:CataloguePath"]))
            {
                var image = System.IO.File.OpenRead(file);
                return File(image, "image/jpeg");
            }

            return new EmptyResult();
        }
    }
}