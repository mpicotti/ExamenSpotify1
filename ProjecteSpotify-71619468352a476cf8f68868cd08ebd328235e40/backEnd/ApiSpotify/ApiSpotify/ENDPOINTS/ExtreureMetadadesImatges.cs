using ApiSpotify.MODELS;
using ApiSpotify.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSpotify.ENDPOINTS
{
    public static class ExtreureMetadadesImatges
    {

        public static void MapExtreureMetadadesImatgesEndpoints(this WebApplication app, DatabaseConnection dbConn)
        {
            // POST /perfil/upload
            app.MapPost("/pefils/upload", async ([FromForm] IFormFileCollection files) =>
            {
                if (files == null || files.Count == 0)
                    return Results.BadRequest(new { message = "No s'ha rebut cap fitxer." });

                ConcurrentBag<Perfils> imatgesProcessades = new ConcurrentBag<Perfils>();

                var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };

                var fileList = files.ToList();

                await Task.Run(() =>
                {
                    Parallel.ForEach(fileList, options, file =>
                    {
                        string tempPath = null;
                        try
                        {
                            var tempFileName = Path.GetRandomFileName();
                            tempPath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(tempFileName, ".mp3"));

                            using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                file.CopyTo(fs);
                            }

                            using (var tagFile = TagLib.File.Create(tempPath))
                            {
                                var tag = tagFile.Tag;
                                var props = tagFile.Properties;

                                var perfils = new Perfils
                                {
                                    Id = Guid.NewGuid(),
                                    Nom = string.IsNullOrWhiteSpace(tag.Title) ? Path.GetFileNameWithoutExtension(file.FileName) : tag.Title,
                                    Descripcio = (tag.Performers != null && tag.Performers.Length > 0) ? tag.Performers[0] : "Desconegut",
                                    Estat = string.IsNullOrWhiteSpace(tag.Album) ? "Desconegut" : tag.Album,
                                };

                                imatgesProcessades.Add(perfils);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processant {file.FileName}: {ex.Message}");
                        }

                    });
                });

                return Results.Ok(imatgesProcessades);
            }).DisableAntiforgery();


        }










    }
}
