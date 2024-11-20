using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Enums
{
    /// <summary>
    /// Enumera los posibles estados de un proceso en el sistema.
    /// </summary>
    public enum ProcessState
    {
        New,        // El proceso ha sido creado pero aún no está listo para ejecutarse.
        Ready,      // El proceso está listo para ejecutarse y espera a ser asignado a la CPU.
        Running,    // El proceso está actualmente en ejecución.
        Blocked,    // El proceso está esperando por algún recurso o evento.
        Terminated  // El proceso ha finalizado su ejecución.
    }
}
