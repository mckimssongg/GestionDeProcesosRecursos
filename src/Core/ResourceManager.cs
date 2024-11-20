using GestionDeProcesosRecursos.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeProcesosRecursos.src.Core
{
    /// <summary>
    /// Gestiona la asignación y liberación de recursos del sistema.
    /// </summary>
    public class ResourceManager
    {
        // Lista de todos los recursos del sistema
        private List<Resource> resources;

        // Contador para generar identificadores únicos de recursos
        private int resourceCounter;

        // Objeto para sincronización
        private readonly object lockObject = new object();

        /// <summary>
        /// Constructor del gestor de recursos.
        /// </summary>
        public ResourceManager()
        {
            resources = new List<Resource>();
            resourceCounter = 0;
        }

        /// <summary>
        /// Crea y agrega un nuevo recurso al sistema.
        /// </summary>
        /// <param name="name">Nombre o descripción del recurso.</param>
        /// <returns>El recurso creado.</returns>
        public Resource CreateResource(string name)
        {
            lock (lockObject)
            {
                var resource = new Resource(++resourceCounter, name);
                resources.Add(resource);
                return resource;
            }
        }

        /// <summary>
        /// Solicita un recurso específico para un proceso.
        /// </summary>
        /// <param name="resourceId">Identificador del recurso.</param>
        /// <param name="process">Proceso que solicita el recurso.</param>
        /// <returns>Verdadero si el recurso fue asignado; de lo contrario, falso.</returns>
        public bool RequestResource(int resourceId, Process process)
        {
            var resource = resources.FirstOrDefault(r => r.ResourceId == resourceId);
            if (resource != null)
            {
                return resource.RequestResource(process);
            }
            else
            {
                return false; // El recurso no existe
            }
        }

        /// <summary>
        /// Libera un recurso específico.
        /// </summary>
        /// <param name="resourceId">Identificador del recurso.</param>
        public void ReleaseResource(int resourceId)
        {
            var resource = resources.FirstOrDefault(r => r.ResourceId == resourceId);
            if (resource != null)
            {
                resource.ReleaseResource();
            }
        }

        /// <summary>
        /// Obtiene una lista de todos los recursos actuales.
        /// </summary>
        /// <returns>Lista de recursos.</returns>
        public List<Resource> GetAllResources()
        {
            lock (lockObject)
            {
                return new List<Resource>(resources);
            }
        }
    }
}
