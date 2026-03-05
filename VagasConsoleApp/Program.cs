using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using VagasConsoleApp.Config;
using VagasConsoleApp.Data;
using Microsoft.Data.SqlClient;


// Modelagem da vaga
public class Vaga
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Company { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public string company_website { get; set; }
}

// Modelagem do JSON retornado
public class ApiResponse
{
    public List<Vaga> Data { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        AppConfig.Initialize();
        string connStr = AppConfig.GetConnectionString("DefaultConnection");


        string url = "https://apis.codante.io/api/job-board/jobs";

        using HttpClient client = new HttpClient();

        try
        {
            Console.WriteLine("Buscando vagas de emprego...");
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            // Deserializar JSON
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, options);

            if (apiResponse?.Data != null && apiResponse.Data.Count > 0)
            {
                foreach (var vaga in apiResponse.Data)
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine($"ID: {vaga.Id}");
                    Console.WriteLine($"Cargo: {vaga.Title}");
                    Console.WriteLine($"Empresa: {vaga.Company}");
                    Console.WriteLine($"Cidade: {vaga.City}");
                    Console.WriteLine($"Salário: {(vaga.Salary.HasValue ? vaga.Salary.Value.ToString("C") : "Não informado")}");
                    Console.WriteLine($"Requisitos: {vaga.Requirements}");
                    Console.WriteLine($"Descrição: {vaga.Description}");
                    Console.WriteLine($"Company Website: {vaga.company_website}");

                    // Inserir no banco
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        string query = @"
                                IF NOT EXISTS (SELECT 1 FROM Vagas WHERE Id = @Id)
                                INSERT INTO Vagas (Id, Title, Company, City, Salary, Requirements, Description, site)
                                VALUES (@Id, @Title, @Company, @City, @Salary, @Requirements, @Description, @company_website)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", vaga.Id);
                            cmd.Parameters.AddWithValue("@Title", vaga.Title ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Company", vaga.Company ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@City", vaga.City ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Salary", vaga.Salary.HasValue ? vaga.Salary.Value : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Requirements", vaga.Requirements ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Description", vaga.Description ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@company_website", vaga.company_website ?? (object)DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }




                    }
                }
            }
            else
            {
                Console.WriteLine("Nenhuma vaga encontrada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar vagas: {ex.Message}");
        }

        Console.WriteLine("Fim da consulta.");
    }
}