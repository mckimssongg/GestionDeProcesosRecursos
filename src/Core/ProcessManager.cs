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

        private ResourceManager resourceManager;

        /// <summary>
        /// Constructor del gestor de procesos.
        /// </summary>
        /// <param name="quantum">Quantum de tiempo en milisegundos.</param>
        public ProcessManager(int quantum, ResourceManager resourceManager)
        {
            this.quantum = quantum;
            this.resourceManager = resourceManager;
            readyQueue = new Queue<Process>();
            allProcesses = new List<Process>();
            processCounter = 0;
        }

        /// <summary>
        /// Crea un nuevo proceso y lo agrega a la cola de listos.
        /// </summary>
        /// <param name="priority">Prioridad del proceso.</param>
        /// <returns>El proceso creado.</returns>
        public Process CreateProcess(int priority)
        {
            lock (lockObject)
            {
                var process = new Process(++processCounter, priority);
                process.ControlBlock.State = ProcessState.Ready;
                allProcesses.Add(process);
                readyQueue.Enqueue(process);
                return process;
            }
        }

        /// <summary>
        /// Inicia la planificación y ejecución de los procesos.
        /// </summary>
        public void StartScheduling()
        {
            System.Threading.Thread schedulingThread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    Process currentProcess = null;

                    lock (lockObject)
                    {
                        if (readyQueue.Any())
                        {
                            currentProcess = readyQueue.Dequeue();
                            currentProcess.ControlBlock.State = ProcessState.Running;
                        }
                    }

                    if (currentProcess != null)
                    {
                        // Iniciar los hilos del proceso
                        currentProcess.Start();

                        // Simular solicitud de recurso
                        bool resourceAcquired = resourceManager.RequestResource(1, currentProcess);
                        if (!resourceAcquired)
                        {
                            // Si el recurso no está disponible, bloquear el proceso
                            currentProcess.ControlBlock.State = ProcessState.Blocked;
                            // Reagregar a la cola de listos después de un tiempo
                            System.Threading.Thread.Sleep(1000);
                            lock (lockObject)
                            {
                                currentProcess.ControlBlock.State = ProcessState.Ready;
                                readyQueue.Enqueue(currentProcess);
                            }
                            continue;
                        }

                        // Simular ejecución por quantum de tiempo
                        System.Threading.Thread.Sleep(quantum);

                        // Liberar el recurso
                        resourceManager.ReleaseResource(1);

                        // Detener los hilos del proceso
                        currentProcess.Terminate();

                        // Simular avance del contador de programa
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
                        }
                    }
                    else
                    {
                        // Si no hay procesos listos, esperar antes de verificar nuevamente
                        System.Threading.Thread.Sleep(100);
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
                // Liberar recursos y memoria asignados
                process.ControlBlock.AllocatedResources.Clear();
                process.ControlBlock.MemoryStart = 0;
                process.ControlBlock.MemorySize = 0;
            }
        }

        /// <summary>
        /// Simula la ejecución del proceso y determina si ha terminado.
        /// </summary>
        /// <param name="process">Proceso a simular.</param>
        /// <returns>Verdadero si el proceso ha terminado; de lo contrario, falso.</returns>
        private bool SimulateProcessExecution(Process process)
        {
            // Lógica simulada: terminar el proceso después de un cierto tiempo
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
    }
}
