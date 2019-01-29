using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoTPV.Model
{
    [Table("Reservas")]
    [AddINotifyPropertyChangedInterface]
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
