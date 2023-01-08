﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Taxes_Data.Models
{
    public partial class TaxesRegister
    {
    
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required"),
            StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be minimum 3 characters")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "LastName is required"), StringLength(100, MinimumLength = 3, ErrorMessage = "LastName should be minimum 3 characters")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Debe ingresar un no telefono")]
        public string NoTelefono { get; set; }

        [Required(ErrorMessage = "Debe de ingresar un correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe de ingresar una fecha de cita")]
        public DateTime? FechaRegistro { get; set; }
    }
}
