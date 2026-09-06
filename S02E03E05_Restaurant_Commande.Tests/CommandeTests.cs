using Restaurant;
namespace Restaurant.Tests;

public class CommandeTests
{
    [Fact]
    public void estVide_SousTotal0m_0Articles()
    {
        //ARANGE
        Commande commande = new Commande("10");
        //ACT
        bool result = commande.EstVide;
        //ASSERT
        Assert.True(result);
    }
    [Fact]
    public void sousTotal_3Article_TrenteDeuxEtDixDisieme()
    {
        //ARANGE
        Commande commande = new Commande("10");
        commande.AjouterLigne(new LigneCommande("C-10", "spag", 15, 3, 0));
        decimal expected = 45;
        //ACT
        decimal result = commande.SousTotal;
        //ASSERT
        Assert.Equal(result, expected);
    }
    [Fact]
    public void sousTotal_1ArticleAvecRabais_15()
    {
        //ARANGE
        Commande commande = new Commande("10");
        commande.AjouterLigne(new LigneCommande("C-10", "spag", 20, 1, 25));
        decimal expected = 15;
        //ACT
        decimal result = commande.SousTotal;
        //ASSERT
        Assert.Equal(result, expected);
    }
    [Theory]
    [InlineData("C-10","spag",-10,1,0,"prixUnitaire")]
    [InlineData("C-10","spag",10,0,0,"quantite")]
    [InlineData("C-10","spag",10,1,-10, "pourcentageRabais")]
    [InlineData("C-10", "spag", 10, 1, 110, "pourcentageRabais")]
    public void ajouteLigne_AvecUnArgumentInvalide_ArgumentOutOfRangeException(string codePlat,string descriptionPlat,decimal prixUnitaire, int quantite, decimal rabaix, string exceptionParamNameExpected)
    {
        // ARRANGE
        Commande commande = new Commande("10");

        // ACT et ASSERT
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => new LigneCommande(codePlat,descriptionPlat,prixUnitaire,quantite,rabaix));
        Assert.Equal(exceptionParamNameExpected, exception.ParamName);
    }

    [Fact]
    public void ajouteLigne_LigneNUll_ArgumentException()
    {
        // ARRANGE
        Commande commande = new Commande("10");
        string expectedExceptionParamName = "codePlat";
        // ACT et ASSERT
        ArgumentException exception = Assert.Throws<ArgumentException>(() => commande.AjouterLigne(new LigneCommande("", "spag", 10, 1, 0)));
        Assert.Equal(expectedExceptionParamName, exception.ParamName);
    }

    [Fact]
    public void ajouteLigne_LigneNUll_ArgumentNullException()
    {
        // ARRANGE
        Commande commande = new Commande("10");
        string expectedExceptionParamName = "ligne";
        // ACT et ASSERT
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => commande.AjouterLigne(null));
        Assert.Equal(expectedExceptionParamName, exception.ParamName);
    }
}
