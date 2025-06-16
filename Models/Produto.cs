using System.ComponentModel.DataAnnotations;

namespace produtos_api.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que 1")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public string? Descricao { get; set; }
    }
}