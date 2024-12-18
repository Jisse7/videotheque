# Installation et Configuration du Projet

## Prérequis
- Visual Studio
- SQL Server Management Studio
- Git

## Installation

0.Préparer le terrain : 
- Créer un dossier sur votre ordinateur
- Ouvrir le terminal 
- Aller dans le dossier nouvellement créé avec la commande : 

```bash
cd VotreDossierCrééSurVotreOrdinateur 
```

1. Cloner le projet :
```bash
git clone https://github.com/Jisse7/videotheque.git
```

2. Ouvrir le projet :
- Lancer Visual Studio
- SELECTIONNER OUVRIR UN PROJET OU UNE SOLUTION
- Ouvrir le fichier .sln du projet qui se trouve dans /videotheque ; celui-ci se nomme videotheque.sln, c'est le premier ; NE PAS SELECTIONNER celui qui se trouve plus bas dans l'arborescence.

3. Configurer la base de données :
- Ouvrir SQL Server Management Studio
-à l'ouverture il vous indiqué le Server name, copiez-le.
- Ouvrir le fichier `appsettings.json` du projet.
- Modifier la chaîne de connexion en remplaçant "VOTRE_SERVER_NAME" par votre Server name indiqué sur SQL Server ManagementStudio, voir en dessous :
```json
"ConnectionStrings": {
    "Default": "Server=VOTRE_SERVER_NAME;Database=VOTRE_BASE_POUR_TESTER_VIDEOTHEQUE;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

4. Dans la Console du Gestionnaire de Package (Tools (outils) > NuGet Package Manager > Package Manager Console) :
```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

IL se peut que cette étape ne soit pas nécessaire et qu'il y est un échec, passez à l'étape 5.

5. Préparer la base de données :

-Toujours dans le nugget manager 

```powershell
Remove-Migration
Add-Migration videotheque_Jc_Yanis
Update-Database
```

6. Seeder la bdd :

-Aller Dans SQL Server Management  : 
-Aller dans Databases
-Votre trouverez VOTRE_BASE_POUR_TESTER_VIDEOTHEQUE
-Faire un clique droit dessus
-New Query
-Copier le contenu du fichier Seed.txt  puis coller dans le fenêtre pour la requête.
-Exécuter la requête.

7. Lancer le projet :
- Appuyer sur F5 ou utiliser le bouton pour exécuter le projet 

8. Se connecter : 
En tant qu'administrateur : 
Login : admin@admin
Mdp : 12345678

En tant que client : 
Créer un compte

(NB: l'admin a les mêmes fonctionnalités que le client en plus des siennes)

