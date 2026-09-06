using Restaurant;

namespace Restaurant.Tests
{
    
    public class ExpediteurCuisineSimulacreUnEnvoi(
        string numeroCommande,
        int nombreArticles,
        decimal montantTotal
        ) : IExpediteurCuisine
    {

        public string m_numeroCommandeAttuendu = numeroCommande;
        public int m_nombreArticlesAttuendu = nombreArticles;
        public decimal m_montantTotalAttuendu = montantTotal;
        public string? numeroCommande;
        public int? nombreArticles;
        public decimal? montantTotal;
        public int nombreAppel = 0;
        
        public void Envoyer(
            string numeroCommande,
            int nombreArticles,
            decimal montantTotal)
        {
            nombreAppel++;
            this.numeroCommande = numeroCommande;
            this.nombreArticles = nombreArticles;
            this.montantTotal = montantTotal;
        }
        public void verifierAttentes()
        {
            Assert.Equal(m_numeroCommandeAttuendu,numeroCommande);
            Assert.Equal(m_nombreArticlesAttuendu,nombreArticles);
            Assert.Equal(m_montantTotalAttuendu,montantTotal);
            Assert.Equal(nombreAppel,1);
        }
    }
}
