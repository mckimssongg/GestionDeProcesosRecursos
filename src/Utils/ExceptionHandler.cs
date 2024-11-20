using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Utils
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex)
        {
            // Registro del error (se podría implementar un logger)
            Console.WriteLine($"Error: {ex.Message}");

            // Mostrar mensaje al usuario
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
