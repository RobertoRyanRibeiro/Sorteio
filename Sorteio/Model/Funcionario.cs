using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorteio.Model
{
    public class Funcionario
    {
        public string Nome { get; set; }
        public bool Venceu { get; set; } = false;
        public int ID { get; set; }


        public override string ToString()
        {
            return $"{ID} - {Nome}";
        }
    }
}
