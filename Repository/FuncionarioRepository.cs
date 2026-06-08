using Azure.Data.Tables;
using RH_Azure.Context;
using RH_Azure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RH_Azure.Repository
{
    public class FuncionarioRepository
    {
        private readonly RHContext _context;
        private readonly string _tableConnectionString;
        private const string TableName = "FuncionarioLog";

        public FuncionarioRepository(RHContext context, string tableConnectionString)
        {
            _context = context;
            _tableConnectionString = tableConnectionString;
        }

        // ─── CRUD no SQL ────────────────────────────────────────────────

        public List<Funcionario> GetAll() =>
            _context.Funcionarios.ToList();

        public Funcionario Get(int id) =>
            _context.Funcionarios.Find(id);

        public Funcionario Create(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();
            RegistrarLog(funcionario, TipoAcao.Criou);
            return funcionario;
        }

        public Funcionario Update(Funcionario funcionario)
        {
            var existing = _context.Funcionarios.Find(funcionario.Id);
            if (existing == null) return null;

            existing.Nome = funcionario.Nome;
            existing.Endereco = funcionario.Endereco;
            existing.Ramal = funcionario.Ramal;
            existing.EmailProfissional = funcionario.EmailProfissional;
            existing.Departamento = funcionario.Departamento;
            existing.Salario = funcionario.Salario;
            existing.DataAdmissao = funcionario.DataAdmissao;

            _context.SaveChanges();
            RegistrarLog(existing, TipoAcao.Atualizou);
            return existing;
        }

        public bool Delete(int id)
        {
            var funcionario = _context.Funcionarios.Find(id);
            if (funcionario == null) return false;

            RegistrarLog(funcionario, TipoAcao.Deletou);
            _context.Funcionarios.Remove(funcionario);
            _context.SaveChanges();
            return true;
        }

        // ─── Log no Azure Table Storage ─────────────────────────────────

        private void RegistrarLog(Funcionario funcionario, TipoAcao tipoAcao)
        {
            var tableClient = new TableClient(_tableConnectionString, TableName);
            tableClient.CreateIfNotExists();

            var log = new FuncionarioLog
            {
                TipoAcao = tipoAcao,
                PartitionKey = funcionario.Departamento ?? "SemDepartamento",
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTimeOffset.UtcNow,
                FuncionarioId = funcionario.Id,
                Nome = funcionario.Nome,
                Endereco = funcionario.Endereco,
                Ramal = funcionario.Ramal,
                EmailProfissional = funcionario.EmailProfissional,
                Departamento = funcionario.Departamento,
                Salario = funcionario.Salario,
                DataAdmissao = funcionario.DataAdmissao
            };

            tableClient.AddEntity(log);
        }
    }
}
