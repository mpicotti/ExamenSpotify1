using ApiSpotify.MODELS;
using ApiSpotify.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ApiSpotify.REPOSITORY
{
    public class DAOPerfils
    {
        public static void Insert(DatabaseConnection dbConn, Perfils perfil)
        {

            dbConn.Open();

            string sql = @"INSERT INTO Users (Id, id_user, nom, descripcio, estat)
                           VALUES (@Id, @id_user, @nom, @descripcio, @estat)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);

            cmd.Parameters.AddWithValue("@Id", perfil.Id);
            cmd.Parameters.AddWithValue("@id_user", perfil.IdUsuari);
            cmd.Parameters.AddWithValue("@nom", perfil.Nom);
            cmd.Parameters.AddWithValue("@descripcio", perfil.Descripcio);
            cmd.Parameters.AddWithValue("@estat", perfil.Estat);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila inserida.");

            dbConn.Close();
        }

        public static List<Perfils> GetAll(DatabaseConnection dbConn)
        {
            List<Perfils> perfils = new();
            dbConn.Open();

            string sql = "SELECT Id, id_user, nom, descripcio, estat FROM Perfils";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                perfils.Add(new Perfils
                {
                    Id = reader.GetGuid(0),
                    IdUsuari = reader.GetGuid(1),
                    Nom = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                    Estat = reader.GetString(3)
                });
            }

            dbConn.Close();
            return perfils;
        }

        public static Perfils? GetById(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = "SELECT Id, nom, contrasenya, salt FROM Users WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            Perfils? perfils = null;

            if (reader.Read())
            {
                perfils = new Perfils
                {
                    Id = reader.GetGuid(0),
                    IdUsuari = reader.GetGuid(1),
                    Nom = reader.GetString(1),
                    Descripcio = reader.GetString(2),
                    Estat = reader.GetString(3)
                };
            }

            dbConn.Close();
            return perfils;
        }

        public static void Update(DatabaseConnection dbConn, Perfils perfil)
        {
            dbConn.Open();

            string sql = @"UPDATE Playlist
                           SET Nom = @nom,
                            id_user = @id_user,
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);

            cmd.Parameters.AddWithValue("@Id", perfil.Id);
            cmd.Parameters.AddWithValue("@id_user", perfil.IdUsuari);
            cmd.Parameters.AddWithValue("@nom", perfil.Nom);
            cmd.Parameters.AddWithValue("@descripcio", perfil.Descripcio);
            cmd.Parameters.AddWithValue("@estat", perfil.Estat);

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"{rows} fila actualitzada.");

            dbConn.Close();
        }


        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Perfils WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            dbConn.Close();

            return rows > 0;
        }

    }
}
