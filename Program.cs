using System;
using System.IO;
using NUnit.Framework;

namespace SistemaCafe
{
    public class DispensadorCafe
    {
        public enum TipoVaso { Chico = 3, Medio = 5, Grande = 7 }
        
        private int cantidadVasos = 10;
        private int stockAzucar = 20;
        private int stockCafe = 50;
        private string reportePath = "reporte_pruebas.txt";
        
        public string PrepararCafe(TipoVaso tipo, int cucharadasAzucar)
        {
            if (cantidadVasos <= 0)
                return GenerarReporte("Error: No quedan vasos");
            
            if (stockCafe < (int)tipo)
                return GenerarReporte("Error: Cafe insuficiente");
            
            if (stockAzucar < cucharadasAzucar)
                return GenerarReporte("Error: Azucar insuficiente");
            
            if (cucharadasAzucar < 0)
                return GenerarReporte("Error: Cantidad de azucar invalida");
            
            cantidadVasos--;
            stockCafe -= (int)tipo;
            stockAzucar -= cucharadasAzucar;
            
            return GenerarReporte($"Cafe {tipo} servido con {cucharadasAzucar} azucar(es)");
        }
        
        private string GenerarReporte(string mensaje)
        {
            File.AppendAllText(reportePath, DateTime.Now + " - " + mensaje + "\n");
            return mensaje;
        }
    }
    
    [TestFixture]
    public class PruebasDispensador
    {
        private DispensadorCafe dispensador;
        
        [SetUp]
        public void Inicializar()
        {
            dispensador = new DispensadorCafe();
        }
        
        [Test]
        public void DeberiaServirCafeConRecursosSuficientes()
        {
            string resultado = dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Medio, 2);
            Assert.AreEqual("Cafe Medio servido con 2 azucar(es)", resultado);
        }
        
        [Test]
        public void NoDebeServirSiNoHayVasos()
        {
            for (int i = 0; i < 10; i++)
            {
                dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Chico, 1);
            }
            string resultado = dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Chico, 1);
            Assert.AreEqual("Error: No quedan vasos", resultado);
        }
        
        [Test]
        public void NoDebeServirSiNoHayCafe()
        {
            for (int i = 0; i < 10; i++)
            {
                dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Grande, 1);
            }
            string resultado = dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Grande, 1);
            Assert.AreEqual("Error: Cafe insuficiente", resultado);
        }
        
        [Test]
        public void NoDebeServirSiNoHayAzucar()
        {
            for (int i = 0; i < 10; i++)
            {
                dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Medio, 2);
            }
            string resultado = dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Medio, 2);
            Assert.AreEqual("Error: Azucar insuficiente", resultado);
        }
        
        [Test]
        public void NoDebePermitirAzucarNegativa()
        {
            string resultado = dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Medio, -1);
            Assert.AreEqual("Error: Cantidad de azucar invalida", resultado);
        }
        
        [Test]
        public void ServirCafeDebeReducirStockCorrectamente()
        {
            dispensador.PrepararCafe(DispensadorCafe.TipoVaso.Medio, 2);
            
            string reporte = File.ReadAllText("reporte_pruebas.txt");
            Assert.IsTrue(reporte.Contains("Cafe Medio servido con 2 azucar(es)"));
        }
    }
}
