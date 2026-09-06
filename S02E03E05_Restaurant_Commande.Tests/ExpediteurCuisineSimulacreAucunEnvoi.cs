using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Tests
{
    public class ExpediteurCuisineSimulacreAucunEnvoi
    : IExpediteurCuisine
    {
        public int nombreAppel = 0;
        public void Envoyer(
            string numeroCommande,
            int nombreArticles,
            decimal montantTotal)
        {
            nombreAppel++;
        }

        public void VerifierAttentes()
        {
            Assert.Equal(nombreAppel, 0);
        }
    }
}
