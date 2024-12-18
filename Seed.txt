-- c pour désactiver la vérification des contraintes
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL"

-- Supprimer les données des tables dans l'ordre correct
DELETE FROM CartItems;
DELETE FROM LocationDetails;
DELETE FROM Paiements;
DELETE FROM Locations;
DELETE FROM Evaluations;
DELETE FROM AspNetUserRoles;
DELETE FROM AspNetUserClaims;
DELETE FROM AspNetUserLogins;
DELETE FROM AspNetUserTokens;
DELETE FROM AspNetRoleClaims;
DELETE FROM AspNetUsers;
DELETE FROM AspNetRoles;
DELETE FROM ExemplairesDVD;  
DELETE FROM Films;
DELETE FROM Categories;

-- c pour réactiver la vérification des contraintes
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL"


-- Réinitialiser les identités
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Films', RESEED, 0);

-- Insertion des catégories
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (Id, Nom, Description) VALUES 
(1, 'Action', 'Films d''action et d''aventure'),
(2, 'Animation', 'Films d''animation'),
(3, 'Drame', 'Films dramatiques'),
(4, 'Horreur', 'Films d''horreur');
SET IDENTITY_INSERT Categories OFF;

-- Insertion des films 
INSERT INTO Films (Titre, Realisateur, AnneeSortie, Synopsis, ImageUrl, TrailerUrl, NombreExemplaires, ExemplairesDisponibles, ScoreMoyen, DureeMinutes, Prix24h, Prix48h, Prix72h, Prix1Semaine, CategorieId) VALUES
('Godzilla 2', 'Michael Dougherty', 2019, 
'L''agence crypto-zoologique Monarch doit faire face à une vague de monstres titanesques. Cette fois, Godzilla est confronté à des adversaires de taille : Mothra, Rodan et sa némésis ultime, le redoutable King Ghidorah à trois têtes. Alors que ces anciennes super-espèces, jusqu''alors considérées comme chimériques, refont surface, elles se livrent à un combat titanesque, mettant en jeu la survie même de l''humanité.',
'https://cdn.sortiraparis.com/images/80/66131/450280-godzilla-2-roi-des-monstres-bande-annonce.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/QFxN2oDKk0E?si=kzlFfgdQpLAsBkQ3" allowfullscreen></iframe>',
5, 5, 4.2, 132, 4.99, 7.99, 9.99, 14.99, 1),

('Pirates des Caraïbes 3', 'Gore Verbinski', 2007, 
'Le capitaine Barbossa, Will Turner et Elizabeth Swann doivent naviguer en eaux troubles pour sauver Jack Sparrow du royaume des morts. Dans leur quête, nos héros doivent former des alliances improbables et affronter la redoutable Compagnie des Indes Orientales, dirigée par le impitoyable Lord Cutler Beckett. L''avenir même de la piraterie est en jeu, alors que les seigneurs pirates des quatre coins du monde se réunissent pour un ultime conseil.',
'https://thumb.canalplus.pro/http/unsafe/1440x810/filters:quality(80)/img-hapi.canalplus.pro:80/ServiceImage/ImageID/109655301',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/HKSZtp_OGHY?si=1Q7nz0wjdYZlRYep" allowfullscreen></iframe>',
5, 5, 4.7, 168, 4.99, 7.99, 9.99, 14.99, 1),

('Les Indestructibles', 'Brad Bird', 2004, 
'Une famille de super-héros doit sortir de sa retraite pour sauver le monde. Bob Parr, jadis connu sous le nom de Mr. Indestructible, et sa femme Helen, ex-Elastigirl, mènent une vie tranquille avec leurs trois enfants. Mais leur routine bien ordonnée vole en éclats quand une mystérieuse convocation gouvernementale propulse Bob dans une mission secrète sur une île lointaine. Bientôt, toute la famille Parr se retrouve impliquée dans un complot machiavélique, orchestré par un ancien fan devenu super-vilain.',
'https://www.profession-spectacle.com/wp-content/uploads/2018/10/Les-Indestructibles-2-Une-1280x640.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/-UaGUdNJdRQ?si=gmPbJLGF36iJT89G" allowfullscreen></iframe>',
5, 5, 4.9, 115, 3.99, 6.99, 8.99, 12.99, 2),

('Spider-Man 3', 'Sam Raimi', 2007, 
'Peter Parker doit faire face à ses plus grands défis alors qu''il lutte contre ses démons intérieurs. Alors que sa relation avec Mary Jane traverse une période difficile, une substance extraterrestre mystérieuse s''attache à lui, amplifiant ses pouvoirs mais aussi ses pires traits de caractère. Pendant ce temps, il doit faire face à de nouveaux ennemis : le Sandman, un criminel qui peut se transformer en sable et qui pourrait être lié à la mort de son oncle Ben, et Venom, une entité maléfique qui exploite ses faiblesses.',
'https://images.ladepeche.fr/api/v1/images/view/5c33fb383e45462eae6aaf52/large/image.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/e5wUilOeOmg?si=N_8QtlebJvhfrnNM" allowfullscreen></iframe>',
5, 5, 3.8, 139, 4.99, 7.99, 9.99, 14.99, 1),

('12 Hommes en colère', 'Sidney Lumet', 1957, 
'Un juré tente de convaincre les autres membres du jury de l''innocence d''un accusé. Dans une salle étouffante en plein été, douze hommes doivent décider à l''unanimité du sort d''un jeune homme accusé de parricide. Ce qui semblait être une affaire simple devient un drame intense où chaque juré doit confronter ses propres préjugés et valeurs morales.',
'https://m.media-amazon.com/images/S/pv-target-images/b92d2865829416e35e7102a3934a2ee745f3b903a95678710442d4299d86f39c._SX1080_FMjpg_.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/TEN-2uTi2c0?si=TYF93b-kNOkyzL0N" allowfullscreen></iframe>',
5, 5, 4.8, 96, 3.99, 6.99, 8.99, 12.99, 3),

('The Dark Knight', 'Christopher Nolan', 2008, 
'Batman doit affronter le Joker qui sème le chaos dans Gotham City. Alors que la ville semblait enfin sur la voie de la paix grâce aux efforts combinés de Batman, du commissaire Gordon et du procureur Harvey Dent, l''arrivée du Joker bouleverse tout l''équilibre. Ce criminel aussi brillant que dérangé met la ville à feu et à sang, poussant ses habitants et ses héros dans leurs derniers retranchements.',
'https://thumb.canalplus.pro/http/unsafe/1440x810/filters:quality(80)/img-hapi.canalplus.pro:80/ServiceImage/ImageID/52831084',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/UMgb3hQCb08?si=Sg7-8FCGBdfCOdUw" allowfullscreen></iframe>',
5, 5, 4.9, 152, 4.99, 7.99, 9.99, 14.99, 1),

('Lilo et Stitch', 'Chris Sanders', 2002, 
'Une petite fille hawaïenne adopte un chien qui s''avère être un extraterrestre. Lilo, une jeune Hawaïenne solitaire, trouve un ami inattendu en la personne de Stitch, une créature extraterrestre programmée pour détruire mais qui découvre le sens de la famille et de l''amour. À travers leurs aventures, ils apprennent ensemble l''importance de l''ohana (la famille) et de l''acceptation des différences.',
'https://prod-ripcut-delivery.disney-plus.net/v1/variant/disney/DBE5B98B171A2525E88838518C1102E4FCD5DC4B50388374E4E2CE3CCDFCB4B4/scale?width=1200&aspectRatio=1.78&format=webp',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/9OAC55UWAQs?si=2JWC0tmXnqN1jlTJ" allowfullscreen></iframe>',
5, 5, 4.6, 85, 3.99, 6.99, 8.99, 12.99, 2),

('The Judge', 'David Dobkin', 2014, 
'Un avocat retourne dans sa ville natale où son père, le juge local, est soupçonné de meurtre. Ce retour forcé ravive de vieilles tensions familiales et oblige Hank à confronter son passé qu''il avait fui. Alors qu''il tente de défendre son père, avec qui il entretient une relation complexe, il découvre que la vérité est plus nuancée qu''il ne le pensait.',
'https://ds.static.rtbf.be/article/image/1920x1080/9/2/8/7221e5c8ec6b08ef6d3f9ff3ce6eb1d1-1416996755.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/ZBvK6ni97W8?si=Xk8NDiETIaPgUhpD" allowfullscreen></iframe>',
5, 5, 4.3, 141, 4.99, 7.99, 9.99, 14.99, 3),

('The Conjuring', 'James Wan', 2013, 
'Des enquêteurs paranormaux viennent en aide à une famille terrorisée par une présence obscure. Ed et Lorraine Warren, célèbres démonologues, sont appelés pour aider la famille Perron, qui vient d''emménager dans une ferme isolée de Rhode Island. Ce qui commence comme des événements étranges isolés se transforme rapidement en une confrontation terrifiante avec des forces démoniaques.',
'https://beam-images.warnermediacdn.com/BEAM_LWM_DELIVERABLES/d1b146e9-7426-4463-804d-3ca656e38492/410a70aa-f424-415d-aae0-9d04cf71dd9c?host=wbd-images.prod-vod.h264.io&partner=beamcom',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/McOmzgX09wo?si=JFAePAzoqSsIpWSL" allowfullscreen></iframe>',
5, 5, 4.5, 112, 4.99, 7.99, 9.99, 14.99, 4),

('L''Exorciste', 'William Friedkin', 1973, 
'Une mère cherche de l''aide auprès de deux prêtres pour sauver sa fille possédée par une entité démoniaque. Ce qui commence comme des changements comportementaux inquiétants chez la jeune Regan se transforme en une terrifiante manifestation de possession démoniaque. Face à l''impuissance de la médecine moderne, Chris MacNeil se tourne vers l''Église comme dernier recours.',
'https://resize.elle.fr/article_1280/var/plain_site/storage/images/loisirs/livres/news/l-exorciste-l-histoire-vraie-de-la-maison-ayant-inspire-le-livre-et-le-film-4249379/102497746-1-fre-FR/L-Exorciste-l-histoire-vraie-de-la-maison-ayant-inspire-le-livre-et-le-film.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/BU2eYAO31Cc?si=_ztEfXpps07gutxD" allowfullscreen></iframe>',
5, 5, 4.7, 122, 3.99, 6.99, 8.99, 12.99, 4),

('Le Château Ambulant', 'Hayao Miyazaki', 2004, 
'Une jeune femme transformée en vieille dame par une sorcière trouve refuge dans un château magique. Sophie, une jeune chapelière, est mystérieusement transformée en vieille dame par la Sorcière des Landes. Cherchant à briser cette malédiction, elle trouve refuge dans le château ambulant du mystérieux magicien Hauru, une structure fantastique qui se déplace sur des pattes mécaniques.',
'https://wave.fr/images/1916/10/studio-ghibli-parc-chateau-ambulant.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/iwROgK94zcM?si=uyvxyWsWhFwN_F1x" allowfullscreen></iframe>',
5, 5, 4.8, 119, 3.99, 6.99, 8.99, 12.99, 2),

('Suzume', 'Makoto Shinkai', 2022, 
'Une adolescente se lance dans un voyage à travers le Japon pour fermer des portes mystérieuses qui menacent l''équilibre du monde. Suzume découvre une porte ancienne dans des ruines, déclenchant une série d''événements qui la mèneront aux quatre coins du pays. Accompagnée d''un mystérieux jeune homme transformé en chaise pliante, elle doit localiser et fermer des portes dimensionnelles qui laissent échapper des catastrophes dans notre monde.',
'https://www.france.tv/image/vignette_16x9/2500/1406/9/3/3/7d350048-4fa9fee05d6c4483ba29242123c6df8d85228e66ca12257f8e61d94188ed7339.jpg',
'<iframe width="800" height="515" src="https://www.youtube.com/embed/0YVb9GkRvLA?si=M2vy0LvRgoRq05GI" allowfullscreen></iframe>',
5, 5, 4.4, 122, 4.99, 7.99, 9.99, 14.99, 2);

-- Insertion des exemplaires de DVD
INSERT INTO ExemplairesDVD (CodeBarre, EstDansStock, FilmId) VALUES
-- Godzilla 2
('GDZ001', 1, 1),
('GDZ002', 1, 1),
('GDZ003', 1, 1),
('GDZ004', 1, 1),
('GDZ005', 1, 1),

-- Pirates des Caraïbes 3
('8717418140106', 1, 2),  -- Code-barre spécifique 
('POC3002', 1, 2),
('POC3003', 1, 2),
('POC3004', 1, 2),
('POC3005', 1, 2),

-- Les Indestructibles
('8717418005276', 1, 3),  -- Code-barre spécifique 
('INC002', 1, 3),
('INC003', 1, 3),
('INC004', 1, 3),
('INC005', 1, 3),

-- Spider-Man 3
('3333297949545', 1, 4),  -- Code-barre spécifique 
('SM3002', 1, 4),
('SM3003', 1, 4),
('SM3004', 1, 4),
('SM3005', 1, 4),

-- 12 Hommes en colère
('12HEC001', 1, 5),
('12HEC002', 1, 5),
('12HEC003', 1, 5),
('12HEC004', 1, 5),
('12HEC005', 1, 5),

-- The Dark Knight
('TDK001', 1, 6),
('TDK002', 1, 6),
('TDK003', 1, 6),
('TDK004', 1, 6),
('TDK005', 1, 6),

-- Lilo et Stitch
('LS001', 1, 7),
('LS002', 1, 7),
('LS003', 1, 7),
('LS004', 1, 7),
('LS005', 1, 7),

-- The Judge
('TJ001', 1, 8),
('TJ002', 1, 8),
('TJ003', 1, 8),
('TJ004', 1, 8),
('TJ005', 1, 8),

-- The Conjuring
('TC001', 1, 9),
('TC002', 1, 9),
('TC003', 1, 9),
('TC004', 1, 9),
('TC005', 1, 9),

-- L'Exorciste
('EXO001', 1, 10),
('EXO002', 1, 10),
('EXO003', 1, 10),
('EXO004', 1, 10),
('EXO005', 1, 10),

-- Le Château Ambulant
('CHA001', 1, 11),
('CHA002', 1, 11),
('CHA003', 1, 11),
('CHA004', 1, 11),
('CHA005', 1, 11),

-- Suzume
('SUZ001', 1, 12),
('SUZ002', 1, 12),
('SUZ003', 1, 12),
('SUZ004', 1, 12),
('SUZ005', 1, 12);