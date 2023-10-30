#Functionement des paquets de données entre le client et le serveur

##Paquet de données
'''
Client                                               Serveur
|                                                      |
|  Crée le contenu du paquet                           |
|  Ajoute : jeton + nonce                              |
|                                                      |
|  Génère HMAC des données ------------->              |
|                                                      |
|  Signe le paquet et le HMAC                          |
|                                                      |
|  Envoie le paquet signé  ------------->              |
|                                                      |
|                                                      |
|                 <------------------ Vérifie la signature du paquet
|                                                      |
|                 <------------------ Vérifie le HMAC du contenu
|                                                      |
|                 <------------------ Vérifie le nonce (anti-rejeu)
|                                                      |
|                 <------------------ Vérifie le jeton (authentification)
|                                                      |
|                                                      |
|   Traitement selon la réponse du serveur             |
|                                                      |

'''