using Restaurant;
using Moq;
namespace Restaurant.Tests;

public class ServiceFinalisationCommandeTests
{
    [Fact]
    public void Finaliser_CommandeValide_PLatsDisponibleCommandeConfirmerCommandeEnvoyerPasDAutreAppels()
    {
        //Arrange
        Mock<IDisponibilitePlats> mockDisponibilitePlats = new Mock<IDisponibilitePlats>();
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("POU-01", 2))
            .Returns(true);
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("SOU-01", 1))
            .Returns(true);

        Mock<IPasserellePaiement> mockPassrellePaiment = new Mock<IPasserellePaiement>();
        mockPassrellePaiment
            .Setup(d => d.Autoriser("C-1042", 30m))
            .Returns(true);

        Mock<IExpediteurCuisine> mockExpediteurCuisine = new Mock<IExpediteurCuisine>();

        IDisponibilitePlats disponibilitePlats = mockDisponibilitePlats.Object;
        IPasserellePaiement passerellePaiement = mockPassrellePaiment.Object;
        IExpediteurCuisine expediteurCuisine = mockExpediteurCuisine.Object;

        Commande commande = new Commande("C-1042");
        commande.AjouterLigne(new LigneCommande("POU-01", "poutine", 12m, 2, 0));
        commande.AjouterLigne(new LigneCommande("SOU-01", "soupe", 6m, 1, 0));
        ServiceFinalisationCommande service = new ServiceFinalisationCommande(disponibilitePlats, passerellePaiement, expediteurCuisine);
        //Act
        ResultatFinalisationCommande resultat = service.Finaliser(commande);
        //Assert
        Assert.Equal(ResultatFinalisationCommande.CommandeConfirmee, resultat);
        mockDisponibilitePlats.Verify(e => e.EstDisponible("POU-01", 2));
        mockDisponibilitePlats.Verify(e => e.EstDisponible("SOU-01", 1));

        mockPassrellePaiment.Verify(e => e.Autoriser("C-1042", 30));

        mockExpediteurCuisine.Verify(e => e.Envoyer("C-1042", 3, 30));

        mockDisponibilitePlats.VerifyNoOtherCalls();
        mockPassrellePaiment.VerifyNoOtherCalls();
        mockExpediteurCuisine.VerifyNoOtherCalls();

    }

    [Fact]
    public void Finaliser_CommandeInvalide_PremierPlatIndisponible()
    {
        //Arrange
        Mock<IDisponibilitePlats> mockDisponibilitePlats = new Mock<IDisponibilitePlats>();
        Mock<IPasserellePaiement> mockPassrellePaiment = new Mock<IPasserellePaiement>();
        Mock<IExpediteurCuisine> mockExpediteurCuisine = new Mock<IExpediteurCuisine>();

        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("POU-01", 2))
            .Returns(false);

        IDisponibilitePlats disponibilitePlats = mockDisponibilitePlats.Object;
        IPasserellePaiement passerellePaiement = mockPassrellePaiment.Object;
        IExpediteurCuisine expediteurCuisine = mockExpediteurCuisine.Object;

        Commande commande = new Commande("C-1042");
        commande.AjouterLigne(new LigneCommande("POU-01", "poutine", 12m, 2, 0));
        commande.AjouterLigne(new LigneCommande("SOU-02", "soupe", 6m, 1, 0));
        ServiceFinalisationCommande service = new ServiceFinalisationCommande(disponibilitePlats,passerellePaiement,expediteurCuisine);

        //Act
        ResultatFinalisationCommande resultat = service.Finaliser(commande);
        //Assert
        Assert.Equal(ResultatFinalisationCommande.PlatIndisponible,resultat);
        mockDisponibilitePlats.Verify(e => e.EstDisponible("POU-01", 2),Times.Once);
        mockPassrellePaiment.Verify(e => e.Autoriser("C-1042",30m), Times.Never);
        mockExpediteurCuisine.Verify(e => e.Envoyer("C-1042",3,30m), Times.Never);

    }

    [Fact]
    public void Finaliser_CommandeInValide_PaiementRefuser()
    {
        //Arrange
        Mock<IDisponibilitePlats> mockDisponibilitePlats = new Mock<IDisponibilitePlats>();
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("POU-01", 2))
            .Returns(true);
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("SOU-01", 1))
            .Returns(true);

        Mock<IPasserellePaiement> mockPassrellePaiment = new Mock<IPasserellePaiement>();
        mockPassrellePaiment
            .Setup(d => d.Autoriser("C-1042", 30m))
            .Returns(false);

        Mock<IExpediteurCuisine> mockExpediteurCuisine = new Mock<IExpediteurCuisine>();

        IDisponibilitePlats disponibilitePlats = mockDisponibilitePlats.Object;
        IPasserellePaiement passerellePaiement = mockPassrellePaiment.Object;
        IExpediteurCuisine expediteurCuisine = mockExpediteurCuisine.Object;

        Commande commande = new Commande("C-1042");
        commande.AjouterLigne(new LigneCommande("POU-01", "poutine", 12m, 2, 0));
        commande.AjouterLigne(new LigneCommande("SOU-01", "soupe", 6m, 1, 0));
        ServiceFinalisationCommande service = new ServiceFinalisationCommande(disponibilitePlats, passerellePaiement, expediteurCuisine);
        //Act
        ResultatFinalisationCommande resultat = service.Finaliser(commande);
        //Assert
        Assert.Equal(ResultatFinalisationCommande.PaiementRefuse, resultat);
        mockDisponibilitePlats.Verify(e => e.EstDisponible("POU-01", 2));
        mockDisponibilitePlats.Verify(e => e.EstDisponible("SOU-01", 1));

        mockPassrellePaiment.Verify(e => e.Autoriser("C-1042", 30));

        mockExpediteurCuisine.Verify(e => e.Envoyer("C-1042", 3, 30),Times.Never);
    }

    [Fact]
    public void Finaliser_CommandeVide_InvalideOperationException()
    {
        //Arrange
        Mock<IDisponibilitePlats> mockDisponibilitePlats = new Mock<IDisponibilitePlats>();
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("POU-01", 2))
            .Returns(true);
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("SOU-01", 1))
            .Returns(true);

        Mock<IPasserellePaiement> mockPassrellePaiment = new Mock<IPasserellePaiement>();
        mockPassrellePaiment
            .Setup(d => d.Autoriser("C-1042", 30m))
            .Returns(true);

        Mock<IExpediteurCuisine> mockExpediteurCuisine = new Mock<IExpediteurCuisine>();

        IDisponibilitePlats disponibilitePlats = mockDisponibilitePlats.Object;
        IPasserellePaiement passerellePaiement = mockPassrellePaiment.Object;
        IExpediteurCuisine expediteurCuisine = mockExpediteurCuisine.Object;

        Commande commande = new Commande("C-1042");
        ServiceFinalisationCommande service = new ServiceFinalisationCommande(disponibilitePlats, passerellePaiement, expediteurCuisine);
        //Act
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => service.Finaliser(commande));
        //Assert
        Assert.Equal("Une commande vide ne peut pas être finalisée.", exception.Message);
        mockDisponibilitePlats.Verify(e => e.EstDisponible("POU-01", 2), Times.Never);
        mockDisponibilitePlats.Verify(e => e.EstDisponible("SOU-01", 1),Times.Never);
        mockPassrellePaiment.Verify(e => e.Autoriser("C-1042", 30m), Times.Never);
        mockExpediteurCuisine.Verify(e => e.Envoyer("C-1042", 3, 30m), Times.Never);
    }

    [Fact]
    public void Finaliser_ContrainteDArgument_30m()
    {
        //Arrange
        Mock<IDisponibilitePlats> mockDisponibilitePlats = new Mock<IDisponibilitePlats>();
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("POU-01", 2))
            .Returns(true);
        mockDisponibilitePlats
            .Setup(d => d.EstDisponible("SOU-01", 1))
            .Returns(true);

        Mock<IPasserellePaiement> mockPassrellePaiment = new Mock<IPasserellePaiement>();
        mockPassrellePaiment
            .Setup(d => d.Autoriser("C-1042", 30m))
            .Returns(true);

        Mock<IExpediteurCuisine> mockExpediteurCuisine = new Mock<IExpediteurCuisine>();

        IDisponibilitePlats disponibilitePlats = mockDisponibilitePlats.Object;
        IPasserellePaiement passerellePaiement = mockPassrellePaiment.Object;
        IExpediteurCuisine expediteurCuisine = mockExpediteurCuisine.Object;

        Commande commande = new Commande("C-1042");
        commande.AjouterLigne(new LigneCommande("POU-01", "poutine", 12m, 2, 0));
        commande.AjouterLigne(new LigneCommande("SOU-01", "soupe", 6m, 1, 0));
        ServiceFinalisationCommande service = new ServiceFinalisationCommande(disponibilitePlats, passerellePaiement, expediteurCuisine);
        //Act
        service.Finaliser(commande);
        //Assert
        mockPassrellePaiment.Verify(e => e.Autoriser("C-1042",It.Is<decimal>(montant => montant == 30m)));
    }

}
