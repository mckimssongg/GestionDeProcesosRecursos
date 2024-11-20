using GestionDeProcesosRecursos.src.Core;
using GestionDeProcesosRecursos.src.Utils;

namespace GestionDeProcesosRecursos
{
    public partial class MainForm : Form
    {
        private ProcessManager processManager;
        private ResourceManager resourceManager;
        private int quantum = 1000; // Quantum de tiempo en milisegundos
        public MainForm()
        {
            InitializeComponent();
            // Inicializar el ResourceManager y agregar algunos recursos
            resourceManager = new ResourceManager();
            resourceManager.CreateResource("Impresora");
            resourceManager.CreateResource("Scanner");

            // Inicializar el ProcessManager
            processManager = new ProcessManager(quantum, resourceManager);

            // Suscribirse al evento de actualizaci�n de procesos
            processManager.ProcessListUpdated += ProcessManager_ProcessListUpdated;

            // Iniciar la planificaci�n de procesos
            processManager.StartScheduling();

            // Configurar el DataGridView para mostrar los procesos
            ConfigureProcessDataGridView();

            // Actualizar la lista de procesos al iniciar
            UpdateProcessList();
        }

        private void ConfigureProcessDataGridView()
        {
            dataGridViewProcesses.AutoGenerateColumns = false;

            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProcessId",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "State",
                HeaderText = "Estado",
                Width = 100
            });

            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Priority",
                HeaderText = "Prioridad",
                Width = 70
            });

            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProgramCounter",
                HeaderText = "Contador de Programa",
                Width = 150
            });
        }

        private void ProcessManager_ProcessListUpdated()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateProcessList));
            }
            else
            {
                UpdateProcessList();
            }
        }

        private void UpdateProcessList()
        {
            try
            {
                var processes = processManager.GetAllProcesses();
                var processViews = new System.ComponentModel.BindingList<ProcessViewModel>();

                foreach (var process in processes)
                {
                    processViews.Add(new ProcessViewModel
                    {
                        ProcessId = process.ControlBlock.ProcessId,
                        State = process.ControlBlock.State.ToString(),
                        Priority = process.ControlBlock.Priority,
                        ProgramCounter = process.ControlBlock.ProgramCounter
                    });
                }

                dataGridViewProcesses.DataSource = processViews;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void btnCreateProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la prioridad del proceso desde la interfaz (por defecto 1)
                int priority = 1;
                int.TryParse(txtPriority.Text, out priority);

                // Crear el proceso
                var process = processManager.CreateProcess(priority);

                // Agregar un hilo al proceso con una acci�n simulada
                process.CreateThread(() =>
                {
                    // Simulaci�n de trabajo del hilo
                    System.Threading.Thread.Sleep(500);
                });

                MessageBox.Show($"Proceso {process.ControlBlock.ProcessId} creado con prioridad {priority}.", "Proceso Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private void btnTerminateProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewProcesses.CurrentRow != null)
                {
                    int processId = (int)dataGridViewProcesses.CurrentRow.Cells[0].Value;
                    var process = processManager.GetAllProcesses().Find(p => p.ControlBlock.ProcessId == processId);
                    if (process != null)
                    {
                        processManager.TerminateProcess(process);
                        MessageBox.Show($"Proceso {processId} terminado.", "Proceso Terminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
    // Clase para representar los procesos en el DataGridView
    public class ProcessViewModel
    {
        public int ProcessId { get; set; }
        public string State { get; set; }
        public int Priority { get; set; }
        public int ProgramCounter { get; set; }
    }
}
