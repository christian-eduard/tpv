using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("Reservas")]
    public class Reserva : PropertyValidateModel
    {
        public int ReservaId { get; set; }
        public string NombreReserva { get; set; }
        public DateTime FechaHora { get; set; }
        public string Comentarios { get; set; }

        //propiedades de navegacion
        public Mesa Mesa { get; set; }
    }
}
