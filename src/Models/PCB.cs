using GestionDeProcesosRecursos.src.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Models
{
    /// <summary>
    /// Representa el bloque de control de proceso que almacena la información relevante de un proceso.
    /// </summary>
    public class PCB
    {
        // Propiedades principales del PCB
        public int ProcessId { get; private set; }
        public ProcessState State { get; set; }
        public int Priority { get; set; }
        public DateTime ArrivalTime { get; private set; }
        public int ProgramCounter { get; set; }
        public int[] Registers { get; set; }
        public int MemoryStart { get; set; }
        public int MemorySize { get; set; }
        public List<Resource> AllocatedResources { get; private set; }

        /// <summary>
        /// Constructor del PCB.
        /// </summary>
        /// <param name="processId">Identificador del proceso.</param>
        /// <param name="priority">Prioridad del proceso.</param>
        public PCB(int processId, int priority)
        {
            ProcessId = processId;
            Priority = priority;
            State = ProcessState.New;
            ArrivalTime = DateTime.Now;
            ProgramCounter = 0;
            Registers = new int[8]; // Simulamos 8 registros
            AllocatedResources = new List<Resource>();
            MemoryStart = 0;
            MemorySize = 0;
        }

        /// <summary>
        /// Asigna un recurso al proceso.
        /// </summary>
        /// <param name="resource">Recurso a asignar.</param>
        public void AllocateResource(Resource resource)
        {
            AllocatedResources.Add(resource);
        }

        /// <summary>
        /// Libera un recurso del proceso.
        /// </summary>
        /// <param name="resource">Recurso a liberar.</param>
        public void ReleaseResource(Resource resource)
        {
            AllocatedResources.Remove(resource);
        }
    }
}
