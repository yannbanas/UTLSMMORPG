# UTLSMMORPG
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/w/yannbanas/UTLSMMORPG/main)

This is a free and opensource self hosted **universal** and **tiny login server** for **mmorpg** (Unity/Unreal/Stride/...)

### Fonctionnalités essentielles d'un serveur de connexion pour un MMORPG :

- **Authentification** : 
    - [x] Vérification des identifiants des utilisateurs lors de la tentative de connexion.
    - [ ] Protection contre les attaques par force brute.
  
- **Création de compte** :
    - [ ] Enregistrement de nouveaux utilisateurs.
    - [ ] Validation des entrées.
    - [ ] Confirmation par email.

- **Récupération de mot de passe** :
    - [ ] Possibilité de réinitialiser le mot de passe.
    - [ ] Envoi d'un email de réinitialisation.

- **Gestion de session** :
    - [x] Création de sessions après connexion.
    - [ ] Gestion de l'expiration des sessions.
    - [ ] Déconnexion.

- **Sécurité** :
    - [ ] Chiffrement des mots de passe dans la base de données.
    - [ ] Protocole sécurisé pour la transmission des données.
    - [ ] Mesures anti-DDoS.
    - [ ] Vérification de l'intégrité des paquets avec HMAC.

- **Logs/Audits** :
    - [x] Enregistrement des activités et des actions du serveur.

- **Maintenance** :
    - [x] Possibilité de mise en mode maintenance.
    - [x] Notifications de maintenance.
    
    (pas encore de test Xunit)

- **Communication avec d'autres serveurs** :
    - [ ] Redirection vers un serveur de jeu après authentification.
    - [ ] Synchronisation avec d'autres serveurs.

- **Mise à jour de client** :
    - [ ] Vérification de la version du client.
    - [x] Notifications de mise à jour (Ajout d'un packet d'envoie de notification Server -> Client).

- **Gestion des bannissements** :
    - [ ] Vérification des bannissements lors de la connexion.
    - [ ] Information sur la raison et la durée du bannissement.

- **Support multilingue** :
    - [ ] Messages d'erreur et d'information dans différentes langues.
