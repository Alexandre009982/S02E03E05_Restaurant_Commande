using Restaurant;
namespace Restaurant.Tests;

public class ServiceTransmissionCuisineTest
{
    [Fact]
    public void Transmettre_CommandeValide_C1042TroisArticlesTotal35mUnAppel()
    {
        //Arrange
        Commande commande = new Commande("C-1042");
        commande.AjouterLigne(new LigneCommande("Spag","Spag",14.50m,2,0m));
        commande.AjouterLigne(new LigneCommande("Soupe", "Soupe",6.00m,1, 0m));
        ExpediteurCuisineSimulacreUnEnvoi expediteur = new ExpediteurCuisineSimulacreUnEnvoi("C-1042",3,35m);
        ServiceTransmissionCuisine service = new ServiceTransmissionCuisine(expediteur);
        //Act
        service.Transmettre(commande);
        //Assert
        expediteur.verifierAttentes();
    }

    [Fact]
    public void Transmettre_CommandeVide_ArgumentNullException()
    {
        //Arrange
        Commande commande = null;
        ExpediteurCuisineSimulacreAucunEnvoi expediteur = new ExpediteurCuisineSimulacreAucunEnvoi();
        ServiceTransmissionCuisine service = new ServiceTransmissionCuisine(expediteur);
        //Act
        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => service.Transmettre(commande));
        expediteur.VerifierAttentes();
    }
    [Fact]
    public void Transmettre_CommandeNull_InvalidOperationException()
    {
        //Arrange
        Commande commande = new Commande("C-1042");
        ExpediteurCuisineSimulacreAucunEnvoi expediteur = new ExpediteurCuisineSimulacreAucunEnvoi();
        ServiceTransmissionCuisine service = new ServiceTransmissionCuisine(expediteur);
        //Act
        //Assert
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => service.Transmettre(commande));
        expediteur.VerifierAttentes();
    }
}
