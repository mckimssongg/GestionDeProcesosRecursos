using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Models
{
    /// <summary>
    /// Representa un hilo de ejecución dentro de un proceso.
    /// </summary>
    public class Thread
    {
        // Identificador único del hilo
        public int ThreadId { get; private set; }

        // Proceso padre al que pertenece el hilo
        public Process ParentProcess { get; private set; }

        // Hilo interno del sistema
        private System.Threading.Thread internalThread;

        // Contador estático para generar identificadores únicos
        private static int threadCounter = 0;

        /// <summary>
        /// Constructor del hilo.
        /// </summary>
        /// <param name="parentProcess">Proceso al que pertenece el hilo.</param>
        /// <param name="startAction">Acción que ejecutará el hilo.</param>
        public Thread(Process parentProcess, ThreadStart startAction)
        {
            ThreadId = ++threadCounter;
            ParentProcess = parentProcess;
            internalThread = new System.Threading.Thread(startAction);
        }

        /// <summary>
        /// Inicia la ejecución del hilo.
        /// </summary>
        public void Start()
        {
            internalThread.Start();
        }

        /// <summary>
        /// Aborta la ejecución del hilo.
        /// </summary>
        public void Abort()
        {
            if (internalThread.IsAlive)
            {
                internalThread.Abort();
            }
        }
    }
}
