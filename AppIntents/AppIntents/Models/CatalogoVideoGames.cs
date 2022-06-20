using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppIntents.Models
{
    public class CatalogoVideoGames
    {
        public SQLiteConnection Conexion { get; set; }

        public CatalogoVideoGames()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/catalogo.db3";
            Conexion = new SQLiteConnection(ruta);
            Conexion.CreateTable<VideogameT>();
        }

        public IEnumerable<VideogameT> GetAll()
        {
            return Conexion.Table<VideogameT>().ToList();
        }

        public void InsertOrReplace(VideogameT VgT)
        {
            var videogame = Conexion.Find<VideogameT>(VgT.Id);

            if (videogame == null)
            {
                if (VgT.NombreVg != null) { Conexion.Insert(VgT); } 
            }
            else if (VgT.NombreVg != null)
            {
                videogame.NombreVg = VgT.NombreVg;
                videogame.DescripcionVg = VgT.DescripcionVg;
                videogame.PortadaVg = VgT.PortadaVg;
                videogame.FechaSalidaVg = VgT.FechaSalidaVg;

                Conexion.Update(videogame); 
            }
            else { Conexion.Delete(videogame); }
        }
    }
}
