using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Finance.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "O nome deve ser inserido")]
        [MinLength(3, ErrorMessage = "O nome deve conter no mínimo 3 caracteres")]
        [MaxLength(80, ErrorMessage = "O nome deve conter no máximo 80 caracteres")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O email deve ser inserido")]
        [MinLength(10, ErrorMessage = "O email deve conter no mínimo 10 caracteres")]
        [MaxLength(100, ErrorMessage = "O email deve conter no máximo 100 caracteres")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "O Email é inválido, insira outro.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "A senha deve ser inserida")]
        [MinLength(6, ErrorMessage = "A senha deve conter no mínimo 6 caracteres")]
        [MaxLength(80, ErrorMessage = "A senha deve conter no máximo 80 caracteres")]
        public string UserPasswd { get; set; }

        public DateOnly UserDtcad { get; set; } 
    }
}
