using ApiCatalogoJogos.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("9546482E-887A-4CAB-A403-AD9C326FFDA5"), new Jogo {Id = Guid.Parse("9546482E-887A-4CAB-A403-AD9C326FFDA5"), Nome="Teste1", Produtora ="Produtora 1", Preco=1.90}},
            {Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), new Jogo {Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), Nome="Teste2", Produtora ="Produtora 2", Preco=1.90}},
            {Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), new Jogo {Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"), Nome="Teste3", Produtora ="Produtora 3", Preco=1.90}}
        };
        
        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }
        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;

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
                    retorno.Add(jogo);
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
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;

        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Sera implementado
        }

    }
}
