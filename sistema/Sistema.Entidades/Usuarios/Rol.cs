using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Entidades.Usuarios
{
    public class Rol
    {
        public int idrol { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3, ErrorMessage = "El nombre no debe de tener mas de 50 caracteres, y menos de 3 caracteres")]
        public string nombre { get; set; }
        [StringLength(256)]
        public string descripcion { get; set; }
        public bool condicion { get; set; }
        /// <summary>
        /// Obtener el listado de los usuarios que se le ha asignado este rol especifico
        /// </summary>
        public ICollection<Usuario> usuarios { get; set; }
    }
}
