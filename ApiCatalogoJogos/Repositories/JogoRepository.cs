using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            { Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"), new Jogo { Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"), Nome = "Fifa 21", Produtora = "EA", Preco = 299} },
            { Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new Jogo { Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"), Nome = "Fifa 20", Produtora = "EA", Preco = 259} },
            { Guid.Parse("e3b4248d-c528-4b9c-9fc2-ef5aaeb529e1"), new Jogo { Id = Guid.Parse("e3b4248d-c528-4b9c-9fc2-ef5aaeb529e1"), Nome = "Fifa 19", Produtora = "EA", Preco = 199} },
            { Guid.Parse("b4dcc3fc-7025-496a-ac28-6cf45bab8737"), new Jogo { Id = Guid.Parse("b4dcc3fc-7025-496a-ac28-6cf45bab8737"), Nome = "Fifa 18", Produtora = "EA", Preco = 159} },
            { Guid.Parse("53eceea0-cf44-4409-b183-34143782be53"), new Jogo { Id = Guid.Parse("53eceea0-cf44-4409-b183-34143782be53"), Nome = "Elden Rings", Produtora = "Xbox", Preco = 299} },
            { Guid.Parse("aefc82c8-05ce-4f0e-a064-87fdd9fa9370"), new Jogo { Id = Guid.Parse("aefc82c8-05ce-4f0e-a064-87fdd9fa9370"), Nome = "BFV", Produtora = "Dice", Preco = 199} },
        };

        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina -1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
            {
                return null;
            }

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                {
                    retorno.Add(jogo);
                }
            }

            return Task.FromResult(retorno);
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] =  jogo;
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão do banco
        }
    }
}
