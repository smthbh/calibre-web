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
using Microsoft.Extensions.Logging;

namespace CalibreWeb.Controllers
{
    public class DownloadController : Controller
    {
        private IConfiguration configuration;
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(IConfiguration configuration, ILogger<DownloadController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index(string path)
        {
            path = System.Text.Encoding.Default.GetString(Convert.FromBase64String(path));
            path = Path.GetFullPath(Path.Combine(configuration["Calibre:CataloguePath"], path));
            _logger.LogInformation($"Request for book full path: {path}", path);

            if (System.IO.File.Exists(path) && path.StartsWith(configuration["Calibre:CataloguePath"]))
            {
                _logger.LogInformation($"Sending file: {path}", path);
                var image = System.IO.File.OpenRead(path);
                return File(image, GetMimeType(Path.GetExtension(path)), Path.GetFileName(path));
            }

            return new NotFoundResult();
        }

        private string GetMimeType(string ext)
        {
            switch (ext.ToLower())
            {
                case ".mobi":
                    return "application/x-mobipocket-ebook";
                case ".epub":
                    return "application/epub+zip";
                case ".azw3":
                    return "application/x-mobi8-ebook";
                case ".zip":
                    return "application/zip";
                case ".pdf":
                    return "application/pdf";
            }
            return "binary/octet-stream";
        }
    }
}