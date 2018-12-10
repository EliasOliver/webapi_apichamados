using Projeto01_ApiChamados.Dados;
using Projeto01_ApiChamados.Enumerações;
using Projeto01_ApiChamados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projeto01_ApiChamados.Controllers
{
    public class ChamadosController : ApiController
    {
        static readonly ChamadosDao dao = new ChamadosDao();

        public IEnumerable<Clientes> GetChamado()
        {
            return dao.BuscarTodos();
        }

        public Clientes GetChamados(int ChamadoId)
        {
            return dao.BuscarChamados(ChamadoId);
        }

        public HttpResponseMessage PostChamado(Clientes clientes)
        {
            ResultadoClientes resultado = dao.EfetuarChamado(clientes);

            if (resultado == ResultadoClientes.DESCRICAO_OK)
            {
                var response = Request.CreateResponse<Clientes>(
                    HttpStatusCode.Created, clientes);

                string uri = Url.Link("DefaultApi", new { id = clientes.ChamadoId });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                string mensagem;

                switch (resultado)
                {
                    case ResultadoClientes.CPF_INVALIDO:
                        mensagem = "O CPF informado não existe";
                        break;
                    default:
                        mensagem = "Ocorreu um erro inesperado";
                        break;
                }


                var erro = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro no servidor"),
                    ReasonPhrase = mensagem
                };
                throw new HttpResponseException(erro);
            }
        }

        public HttpResponseMessage PutChamado(int ChamadoId, Clientes clientes)
        {
            clientes.ChamadoId = ChamadoId;
            ResultadoClientes resultado = dao.AlterarChamado(clientes);

            if (resultado == ResultadoClientes.DESCRICAO_OK)
            {
                var response = Request.CreateResponse<Clientes>(
                    HttpStatusCode.Created, clientes);

                string uri = Url.Link("DefaultApi", new { id = clientes.ChamadoId });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                string mensagem;

                switch (resultado)
                {
                    case ResultadoClientes.CPF_INVALIDO:
                        mensagem = "O CPF informado não existe";
                        break;
                    case ResultadoClientes.CHAMADO_RESPONDIDO:
                        mensagem = "Este chamado já foi respondido antes";
                        break;
                    case ResultadoClientes.CHAMADO_NAO_REALIZADO:
                        mensagem = "Impossível alterar: chamado não existe";
                        break;
                    case ResultadoClientes.CHAMADO_EXCLUIDO:
                        mensagem = "Impossível alterar: Este chamado foi excluido";
                        break;
                    default:
                        mensagem = "Ocorreu um erro inesperado";
                        break;
                }


                var erro = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro no servidor"),
                    ReasonPhrase = mensagem
                };
                throw new HttpResponseException(erro);
            }
        }
    }
}
