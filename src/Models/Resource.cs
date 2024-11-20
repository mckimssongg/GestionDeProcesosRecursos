using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Models
{
    /// <summary>
    /// Representa un recurso del sistema que puede ser asignado a procesos.
    /// </summary>
    public class Resource
    {
        // Identificador único del recurso
        public int ResourceId { get; private set; }

        // Nombre o descripción del recurso
        public string Name { get; private set; }

        // Indica si el recurso está actualmente asignado
        public bool IsAllocated { get; private set; }

        // Proceso al que está asignado el recurso
        public Process AllocatedTo { get; private set; }

        // Objeto para sincronización
        private readonly object lockObject = new object();

        /// <summary>
        /// Constructor del recurso.
        /// </summary>
        /// <param name="resourceId">Identificador único del recurso.</param>
        /// <param name="name">Nombre o descripción del recurso.</param>
        public Resource(int resourceId, string name)
        {
            ResourceId = resourceId;
            Name = name;
            IsAllocated = false;
            AllocatedTo = null;
        }

        /// <summary>
        /// Solicita el recurso para un proceso.
        /// </summary>
        /// <param name="process">Proceso que solicita el recurso.</param>
        /// <returns>Verdadero si el recurso fue asignado; de lo contrario, falso.</returns>
        public bool RequestResource(Process process)
        {
            lock (lockObject)
            {
                if (!IsAllocated)
                {
                    IsAllocated = true;
                    AllocatedTo = process;
                    process.ControlBlock.AllocateResource(this);
                    return true;
                }
                else
                {
                    return false; // El recurso ya está asignado
                }
            }
        }

        /// <summary>
        /// Libera el recurso del proceso al que está asignado.
        /// </summary>
        public void ReleaseResource()
        {
            lock (lockObject)
            {
                if (IsAllocated && AllocatedTo != null)
                {
                    AllocatedTo.ControlBlock.ReleaseResource(this);
                    IsAllocated = false;
                    AllocatedTo = null;
                }
            }
        }
    }
}
