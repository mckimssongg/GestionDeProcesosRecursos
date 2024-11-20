using GestionDeProcesosRecursos.src.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Models
{
    /// <summary>
    /// Representa un proceso en el sistema operativo simulado.
    /// </summary>
    public class Process
    {
        // Bloque de control de proceso asociado
        public PCB ControlBlock { get; private set; }

        // Lista de hilos asociados al proceso
        public List<ProcessThread> Threads { get; private set; }

        /// <summary>
        /// Constructor del proceso.
        /// </summary>
        /// <param name="processId">Identificador único del proceso.</param>
        /// <param name="priority">Prioridad del proceso.</param>
        public Process(int processId, int priority)
        {
            ControlBlock = new PCB(processId, priority);
            Threads = new List<ProcessThread>();
        }

        /// <summary>
        /// Crea y agrega un nuevo hilo al proceso.
        /// </summary>
        /// <param name="startAction">Acción que ejecutará el hilo.</param>
        public void CreateThread(System.Threading.ThreadStart startAction)
        {
            var thread = new ProcessThread(this, startAction);
            Threads.Add(thread);
        }

        /// <summary>
        /// Inicia la ejecución de todos los hilos del proceso.
        /// </summary>
        public void Start()
        {
            foreach (var thread in Threads)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Detiene la ejecución de todos los hilos del proceso sin terminarlo.
        /// </summary>
        public void StopThreads()
        {
            foreach (var thread in Threads)
            {
                thread.Stop();
            }
        }

        /// <summary>
        /// Reanuda la ejecución de todos los hilos del proceso.
        /// </summary>
        public void ResumeThreads()
        {
            foreach (var thread in Threads)
            {
                thread.Resume();
            }
        }

        /// <summary>
        /// Termina el proceso y todos sus hilos.
        /// </summary>
        public void Terminate()
        {
            ControlBlock.State = ProcessState.Terminated;
            //foreach (var thread in Threads)
            //{
            //    thread.Abort();
            //}
            StopThreads();
            // Liberar recursos y memoria asociados
            ControlBlock.AllocatedResources.Clear();
            ControlBlock.MemoryStart = 0;
            ControlBlock.MemorySize = 0;
        }
    }
}
