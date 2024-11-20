using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Models
{
    /// <summary>
    /// Representa un hilo de ejecución dentro de un proceso.
    /// </summary>
    public class ProcessThread
    {
        // Identificador único del hilo
        public int ThreadId { get; private set; }

        // Proceso padre al que pertenece el hilo
        public Process ParentProcess { get; private set; }

        private ThreadStart startAction;

        // Hilo interno del sistema
        private System.Threading.Thread internalThread;

        // Contador estático para generar identificadores únicos
        private static int threadCounter = 0;

        private ManualResetEventSlim suspendEvent = new ManualResetEventSlim(true);

        // Token de cancelación
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        /// <summary>
        /// Constructor del hilo.
        /// </summary>
        /// <param name="parentProcess">Proceso al que pertenece el hilo.</param>
        /// <param name="startAction">Acción que ejecutará el hilo.</param>
        public ProcessThread(Process parentProcess, ThreadStart startAction)
        {
            ThreadId = ++threadCounter;
            ParentProcess = parentProcess;
            this.startAction = startAction;

            // Inicializar los recursos
            InitializeThreadResources();

            //// Inicializar el token de cancelación
            //cancellationTokenSource = new CancellationTokenSource();
            //cancellationToken = cancellationTokenSource.Token;

            //internalThread = new Thread(Run);
        }

        /// <summary>
        /// Inicializa los recursos necesarios para el hilo.
        /// </summary>
        private void InitializeThreadResources()
        {
            // Inicializar el token de cancelación
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            // Inicializar el evento de suspensión
            suspendEvent = new ManualResetEventSlim(true);

            // Crear el hilo interno
            internalThread = new Thread(Run);
        }

        private void Run()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Esperar hasta que el hilo sea reanudado
                suspendEvent.Wait(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                // Ejecutar la acción del hilo
                startAction();

                // Opcional: Añadir una pausa pequeña para evitar un bucle demasiado rápido
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Inicia la ejecución del hilo.
        /// </summary>
        public void Start()
        {
            if (internalThread == null || !internalThread.IsAlive)
            {
                // Si el hilo ha terminado o aún no ha sido iniciado, inicializar recursos y empezar
                InitializeThreadResources();
                internalThread.Start();
            }
            else
            {
                // Si el hilo está en espera, reanudarlo
                Resume();
            }
        }

        /// <summary>
        /// Detiene la ejecución del hilo.
        /// </summary>
        public void Stop()
        {
            if (internalThread != null && internalThread.IsAlive)
            {
                // Solicitar la cancelación
                cancellationTokenSource.Cancel();

                // Asegurarnos de que el hilo no esté suspendido para que pueda terminar
                suspendEvent.Set();

                // Esperar a que el hilo termine
                internalThread.Join();

                // Liberar recursos
                internalThread = null;
                cancellationTokenSource.Dispose();
                suspendEvent.Dispose();
            }
        }

        /// <summary>
        /// Suspende la ejecución del hilo.
        /// </summary>
        public void Suspend()
        {
            if (internalThread != null && internalThread.IsAlive)
            {
                suspendEvent.Reset();
            }
        }

        /// <summary>
        /// Reanuda la ejecución del hilo.
        /// </summary>
        public void Resume()
        {
            if (internalThread != null && internalThread.IsAlive)
            {
                suspendEvent.Set();
            }
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
