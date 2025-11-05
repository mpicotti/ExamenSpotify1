using ApiSpotify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiSpotify.MODELS;
using ApiSpotify.REPOSITORY;
using Microsoft.AspNetCore.Mvc;

namespace ApiSpotify.ENDPOINTS
{
    public static class EndpointPerfils
    {
        public static void MapPerfilsEndpoints(this WebApplication app, DatabaseConnection dbConn)
        {
            app.MapGet("/perfils", () =>
            {
                List<Perfils> perfils = DAOPerfils.GetAll(dbConn);
                return Results.Ok(perfils);
            });

            app.MapGet("/perfils/{id}", (Guid id) =>
            {
                Perfils? perfils = DAOPerfils.GetById(dbConn, id);

                return perfils is not null
                    ? Results.Ok(perfils)
                    : Results.NotFound(new { message = $"Perfils with Id {id} not found." });
            });

            app.MapPost("/perfils", (Perfils req) =>
            {
                Perfils perfil = new Perfils
                {
                    Id = Guid.NewGuid(),
                    IdUsuari = req.IdUsuari,
                    Nom = req.Nom,
                    Descripcio = req.Descripcio,
                    Estat = req.Estat,
                };

                DAOPerfils.Insert(dbConn, perfil);
                return Results.Created($"/playlists/{perfil.Id}", perfil);
            });

            app.MapPut("/perfils/{id}", (Guid id, Perfils req) =>
            {
                var existing = DAOPerfils.GetById(dbConn, id);
                if (existing == null)
                    return Results.NotFound();

                Perfils updated = new Perfils
                {
                    Id = Guid.NewGuid(),
                    IdUsuari = req.IdUsuari,
                    Nom = req.Nom,
                    Descripcio = req.Descripcio,
                    Estat = req.Estat,
                };

                DAOPerfils.Update(dbConn, updated);
                return Results.Ok(updated);
            });

            app.MapDelete("/perfils/{id}", (Guid id) =>
                DAOPerfils.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

        }
    }
}
