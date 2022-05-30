using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace MyNotes.Models
{
    public class ListaNotas
    {
        public SQLiteConnection Conexion { get; set; }

        public ListaNotas()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/notas.db3";
            Conexion = new SQLiteConnection(ruta);
            Conexion.CreateTable<Notas>();
        }

        public IEnumerable<Notas> GetAll()
        {
            return Conexion.Table<Notas>().ToList().OrderByDescending(x => x.Id);
        }

        public void InsertOrReplace(Notas nO)
        {
            var nota = Conexion.Find<Notas>(nO.Id);

            if(nota == null)
            {
                if (nO.Contenido != null) { Conexion.Insert(nO); } //Create
            }
            else if (nO.Contenido != null) 
            { 
                nota.Contenido = nO.Contenido; 
                nota.Titulo = nO.Titulo;
                nota.Id = nO.Id;
                Conexion.Update(nota); //Update
            }
            else { Conexion.Delete(nota); } //Delete
        }
    }
}
