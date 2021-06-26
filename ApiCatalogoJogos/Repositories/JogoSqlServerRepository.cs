using ApiCatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection sqlConnection;

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                var jogo = new Jogo();


                jogo.Id = Guid.Parse(sqlDataReader["Id"].ToString());
                jogo.Nome = (string)sqlDataReader["Nome"];
                jogo.Produtora = (string)sqlDataReader["Produtora"];
                jogo.Preco = double.Parse(sqlDataReader["Preco"].ToString());

                jogos.Add(jogo);
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;
            var comando = $"select * from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = double.Parse(sqlDataReader["Preco"].ToString())
                };
            }

            await sqlConnection.CloseAsync();

            return jogo;
        }

        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();
            var comando = $"select * from Jogos where Nome = '{nome}' and Produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            var sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = Guid.Parse(sqlDataReader["Id"].ToString()),
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = double.Parse(sqlDataReader["Preco"].ToString())
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }

        public async Task Inserir(Jogo jogo)
        {
            var comando = $"insert into Jogos (Id, Nome, Produtora, Preco) Values('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', '{jogo.Preco.ToString()}')";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();

            await sqlConnection.CloseAsync();
        }

        public async Task Atualizar(Jogo jogo)
        {
            var comando = $"update Jogos set Nome = '{jogo.Nome}' , Produtora = '{jogo.Produtora}', Preco = '{jogo.Preco.ToString()}' where Id = '{jogo.Id}'";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();

            await sqlConnection.CloseAsync();
        }

        public async Task Remover(Guid id)
        {
            var comando = $"delete from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            var sqlCommand = new SqlCommand(comando, sqlConnection);
            await sqlCommand.ExecuteNonQueryAsync();

            await sqlConnection.CloseAsync();

        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
