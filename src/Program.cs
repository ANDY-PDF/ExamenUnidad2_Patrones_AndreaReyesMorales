// See https://aka.ms/new-console-template for more information

using System;
namespace EXAMEN_U2
{
    //Definir los dos unicos estados
    public enum EstadoEspacio
    {
        Disponible,
        Ocupado
    }

    public class EspacioParking //EspacioParking = Cajon de estacionamiento
    {
        public int Numero { get; private set; }
        public EstadoEspacio Estado { get; set; }

        public EspacioParking(int numero)
        {
            Numero = numero;
            Estado = EstadoEspacio.Disponible;
        }

        public void Ocupar()
        {
            Estado = EstadoEspacio.Ocupado;
            Console.WriteLine($"Espacio {Numero} ocupado.");
        }

        public void Liberar()
        {
            Estado = EstadoEspacio.Disponible;
            Console.WriteLine($"Espacio {Numero} liberado.");
        }

    }

    //Aqui inicia el OBJECT POOl; creamos todos los espcacios del estacionamiento
    public class PoolEspaciosParking
    {
        private readonly List<EspacioParking> espacios;
        public PoolEspaciosParking(int cantidad)
        {
            espacios = new List<EspacioParking>();
            for (int i = 1; i <= cantidad; i++)
            {
                espacios.Add(new EspacioParking(i));
            }
        }

        //Buscar y ocupar un espaico

        public EspacioParking ObtenerEspacioDisponible()
        {
            var espacio = espacios.FirstOrDefault(e => e.Estado == EstadoEspacio.Disponible);
            if (espacio != null)
            {
                espacio.Ocupar();
            }
            else
            {
                Console.WriteLine("No  hay espacios disponibles en este momento");
            }
            return espacio;
        }

        //Liberar un espacio
        public void LiberarEspacio(int numeroEspacio)
        {
            var espacio = espacios.FirstOrDefault(e => e.Numero == numeroEspacio);
            if (espacio != null && espacio.Estado == EstadoEspacio.Ocupado)
            {
                espacio.Liberar();
            }
            else
            {
                Console.WriteLine($"El espacio {numeroEspacio} no esta ocupado o no existe.");
            }
        }

        //Mostrar estado Actual

        public void MostrarEstadoEspacios()
        {
            Console.WriteLine("\n === ESTADO ACTUAL DEL ESTACIONAMIENTO ===");
            foreach (var espacio in espacios)
            {
                string estado = espacio.Estado == EstadoEspacio.Disponible ? "Disponible" : "Ocupado";
                Console.WriteLine($"Espacio {espacio.Numero} {estado}");
            }
            Console.WriteLine("========================================\n");
        }

        public int EspaciosDisponibles => espacios.Count(e => e.Estado == EstadoEspacio.Disponible);
        public int EspaciosOcupados => espacios.Count(e => e.Estado == EstadoEspacio.Ocupado);
    }

    // Aqui inicia SINGLETON; controlador unico
    public class ControladorParking
    {
            private static ControladorParking _instancia;
            private static readonly object _lock = new object();
            private PoolEspaciosParking pool;

            private ControladorParking(int totalEspacios)
            {
                pool = new PoolEspaciosParking(totalEspacios);
                Console.WriteLine($"Controlador de parking inicializado con {totalEspacios} espacios.");
            }

            public static ControladorParking Instancia
            {
                get
                {
                    if (_instancia == null) //Primera verificacion 
                    {
                        lock (_lock) //Solo un carro puede verficiar el estacionamiento a la vez
                        {
                            if ( _instancia == null) //Segunda verificacion
                            {
                                _instancia = new ControladorParking(5);
                            }
                        }
                    }
                    return _instancia; //Todas los carros usan la misma entrada
                }
            }

            public void OcuparEspacio ()
            {
                Console.WriteLine("Solicitando espacio...");
                pool.ObtenerEspacioDisponible();
            }

            public void LiberarEspacio(int numeroEspacio)
            {
                Console.WriteLine($"Liberando espacio {numeroEspacio}...");
                pool.LiberarEspacio(numeroEspacio);
            }

            public void MostrarEstado()
            {
                pool.MostrarEstadoEspacios();
            }
    }

    //Simulacion 
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE ESTACIONAMIENTO ===\n");

            ControladorParking controlador1 = ControladorParking.Instancia;
            ControladorParking controlador2 = ControladorParking.Instancia;

            Console.WriteLine("¿Misma instancia? " + ReferenceEquals(controlador1, controlador2));
            Console.WriteLine();

            controlador1.MostrarEstado();

            Console.WriteLine("=== OCUPANDO ESPACIOS ===");
            controlador1.OcuparEspacio();
            controlador1.OcuparEspacio();
            controlador1.OcuparEspacio();

            controlador1.MostrarEstado();

            Console.WriteLine("=== LIBERANDO ESPACIOS ===");
            controlador1.LiberarEspacio(1);
            controlador1.LiberarEspacio(2);

            controlador1.MostrarEstado();

            Console.WriteLine("=== OCUPANDO MÁS ESPACIOS ===");
            controlador1.OcuparEspacio();
            controlador1.OcuparEspacio();
            controlador1.OcuparEspacio();

            controlador1.MostrarEstado();

            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }

}
