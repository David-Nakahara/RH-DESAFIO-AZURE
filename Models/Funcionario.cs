using System;
using System.ComponentModel.DataAnnotations;

namespace RH_Azure.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Endereco { get; set; }

        public string Ramal { get; set; }

        public string EmailProfissional { get; set; }

        public string Departamento { get; set; }

        public decimal Salario { get; set; }

        public DateTime DataAdmissao { get; set; }
    }
}
