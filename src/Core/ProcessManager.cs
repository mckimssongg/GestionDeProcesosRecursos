using GestionDeProcesosRecursos.src.Enums;
using GestionDeProcesosRecursos.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Core
{
    /// <summary>
    /// Gestiona la creación, planificación y terminación de procesos.
    /// </summary>
    public class ProcessManager
    {
        // Cola de procesos listos para ejecutarse
        private Queue<Process> readyQueue;

        // Lista de todos los procesos en el sistema
        private List<Process> allProcesses;

        // Quantum de tiempo para el algoritmo Round Robin
        private int quantum;

        // Contador para generar identificadores únicos de procesos
        private int processCounter;

        // Objeto para sincronización
        private object lockObject = new object();

        // Evento que se dispara cuando se actualiza la lista de procesos
        public event Action ProcessListUpdated;

        // Indica si la planificación está en ejecución
        private bool isScheduling;

        private ResourceManager resourceManager;

        /// <summary>
        /// Constructor del gestor de procesos.
        /// </summary>
        /// <param name="quantum">Quantum de tiempo en milisegundos.</param>
        /// <param name="resourceManager">Gestor de recursos.</param>
        public ProcessManager(int quantum, ResourceManager resourceManager)
        {
            this.quantum = quantum;
            this.resourceManager = resourceManager;
            readyQueue = new Queue<Process>();
            allProcesses = new List<Process>();
            processCounter = 0;
            isScheduling = false;
        }

        /// <summary>
        /// Crea un nuevo proceso y lo agrega a la cola de listos.
        /// </summary>
        /// <param name="priority">Prioridad del proceso.</param>
        /// <returns>El proceso creado.</returns>
        public Process CreateProcess(int priority)
        {
            Process process;
            lock (lockObject)
            {
                process = new Process(++processCounter, priority);
                process.ControlBlock.State = ProcessState.Ready;
                allProcesses.Add(process);
                readyQueue.Enqueue(process);
                // OnProcessListUpdated(); // <-- Se llama al evento dentro del bloqueo ///// YA NO 
                // return process;
            }
            // Llamar a OnProcessListUpdated() fuera del bloqueo
            OnProcessListUpdated();
            return process;
        }

        /// <summary>
        /// Inicia la planificación y ejecución de los procesos.
        /// </summary>
        public void StartScheduling()
        {
            if (isScheduling)
                return;

            isScheduling = true;

            // Usar System.Threading.Thread por que si no la mamada se confunde
            System.Threading.Thread schedulingThread = new System.Threading.Thread(() =>
            {
                while (isScheduling)
                {
                    Process currentProcess = null;

                    lock (lockObject)
                    {
                        if (readyQueue.Any())
                        {
                            currentProcess = readyQueue.Dequeue();
                            currentProcess.ControlBlock.State = ProcessState.Running;
                            // No llamar a OnProcessListUpdated() aquí
                        }
                    }

                    if (currentProcess != null)
                    {
                        OnProcessListUpdated(); // Llamar fuera del bloqueo

                        // Iniciar los hilos del proceso
                        currentProcess.Start();

                        // Simular ejecución por quantum de tiempo
                        Thread.Sleep(quantum);

                        // Detener los hilos del proceso
                        currentProcess.StopThreads();

                        // "Simular" avance del contador de programa
                        currentProcess.ControlBlock.ProgramCounter += quantum;

                        // Verificar si el proceso ha terminado
                        bool isFinished = SimulateProcessExecution(currentProcess);

                        if (isFinished)
                        {
                            TerminateProcess(currentProcess);
                        }
                        else
                        {
                            lock (lockObject)
                            {
                                currentProcess.ControlBlock.State = ProcessState.Ready;
                                readyQueue.Enqueue(currentProcess);
                            }
                            OnProcessListUpdated();
                        }
                    }
                    else
                    {
                        // Si no hay procesos listos, esperar antes de verificar nuevamente
                        Thread.Sleep(100);
                    }
                }
            });

            schedulingThread.IsBackground = true;
            schedulingThread.Start();
        }

        /// <summary>
        /// Termina un proceso y libera sus recursos.
        /// </summary>
        /// <param name="process">Proceso a terminar.</param>
        public void TerminateProcess(Process process)
        {
            lock (lockObject)
            {
                process.Terminate();
                allProcesses.Remove(process);
            }
            OnProcessListUpdated(); // <-- Se llama al evento fuera del bloqueo por que si no se confunde y nos da un deadlock
        }

        /// <summary>
        /// Simula la ejecución del proceso y determina si ha terminado.
        /// </summary>
        /// <param name="process">Proceso a simular.</param>
        /// <returns>Verdadero si el proceso ha terminado; de lo contrario, falso.</returns>
        private bool SimulateProcessExecution(Process process)
        {
            // Lógica simulada: terminar el proceso después de cierto contador
            if (process.ControlBlock.ProgramCounter >= 5000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene una lista de todos los procesos actuales.
        /// </summary>
        /// <returns>Lista de procesos.</returns>
        public List<Process> GetAllProcesses()
        {
            lock (lockObject)
            {
                return new List<Process>(allProcesses);
            }
        }

        /// <summary>
        /// Dispara el evento de actualización de la lista de procesos.
        /// </summary>
        private void OnProcessListUpdated()
        {
            ProcessListUpdated?.Invoke();
        }
    }
}
