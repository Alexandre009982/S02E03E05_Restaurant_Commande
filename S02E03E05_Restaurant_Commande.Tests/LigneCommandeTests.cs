using Restaurant;
namespace LigneCommandeTest;

public sealed class LigneCommandeTests
{
    [Fact]
    public void CalculerTotal_AvecAucunRabais_29m()
    {
        //ARANGE
        LigneCommande ligneCommande = new LigneCommande("C-1042", "Spag", 14.50m, 2, 0m);
        decimal expected = 29.00m;
        //ACT
        decimal prixFinal = ligneCommande.CalculerTotal();
        //ASSERT
        Assert.Equal(prixFinal, expected);
    }
    [Theory]
    [InlineData("C-1042", "Spag", 10, 1, 0,10)]
    [InlineData("C-1042", "Spag", 12.50, 2, 10,22.50)]
    [InlineData("C-1042", "Spag", 3.25, 4, 20,10.40)]
    [InlineData("C-1042", "Spag", 10, 2, 100,0)]
    public void CalculerTotal_SansRabais_ProduitMontantNet(string code,string description,decimal prix,int quantite,decimal rabais,decimal expected)
    {
        //ARANGE
        LigneCommande ligneCommande = new LigneCommande(code,description,prix,quantite,rabais);
        //ACT
        decimal prixFinal = ligneCommande.CalculerTotal();
        //ASSERT
        Assert.Equal(prixFinal,expected);
    }
}