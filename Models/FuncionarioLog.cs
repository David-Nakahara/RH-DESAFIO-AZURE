using Azure;
using Azure.Data.Tables;
using System;

namespace RH_Azure.Models
{
    public class FuncionarioLog : ITableEntity
    {
        public TipoAcao TipoAcao { get; set; }

        // ITableEntity
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Dados do funcionário copiados para o log
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Ramal { get; set; }
        public string EmailProfissional { get; set; }
        public string Departamento { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
    }
}
