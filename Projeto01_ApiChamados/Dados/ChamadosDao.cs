using Projeto01_ApiChamados.Enumerações;
using Projeto01_ApiChamados.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01_ApiChamados.Dados
{
    public class ChamadosDao
    {
        public IEnumerable<Clientes> BuscarTodos()
        {
            using (var ctx = new ChamadoEntities())
            {
                return ctx.Clientes.ToList();
            }
        }

        public Clientes BuscarChamados(int ChamadoId)
        {
            using (var ctx = new ChamadoEntities())
            {
                return ctx.Clientes.FirstOrDefault(p => p.ChamadoId == ChamadoId);
            }
        }

        public ResultadoClientes EfetuarChamado(Clientes clientes)
        {
            using (var ctx = new ChamadoEntities())
            {
                ctx.Clientes.Add(clientes);
                ctx.SaveChanges();

                return ResultadoClientes.DESCRICAO_OK;
            }
        }

        public ResultadoClientes AlterarChamado(Clientes clientes)
        {
            using (var ctx = new ChamadoEntities())
            {
                var cpf = ctx.Clientes
                    .FirstOrDefault(c => c.CpfCliente.Equals(clientes.CpfCliente));
                if (cpf == null)
                {
                    return ResultadoClientes.CPF_INVALIDO;
                }

                var resposta = ctx.Clientes
                    .Where(c => c.Resposta.Equals(clientes.Resposta));
                if (resposta.Count() == 0) 
                {
                    return ResultadoClientes.CHAMADO_NAO_REALIZADO;
                }

                ctx.Entry<Clientes>(clientes).State = EntityState.Modified;
                ctx.SaveChanges();
                return ResultadoClientes.DESCRICAO_OK;
            }    
        }

        public Clientes RemoverChamado(int ChamadoId)
        {
            using (var ctx = new ChamadoEntities())
            {
                var chamado = ctx.Clientes
                    .FirstOrDefault(p => p.ChamadoId == ChamadoId);
                if (chamado == null)
                {
                    return null;
                }

                ctx.Entry<Clientes>(chamado).State = EntityState.Deleted;
                ctx.SaveChanges();
                return chamado;
            }
        }
    }
}