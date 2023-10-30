#Functionement des paquets de donn�es entre le client et le serveur

##Paquet de donn�es
'''
Client                                               Serveur
|                                                      |
|  Cr�e le contenu du paquet                           |
|  Ajoute : jeton + nonce                              |
|                                                      |
|  G�n�re HMAC des donn�es ------------->              |
|                                                      |
|  Signe le paquet et le HMAC                          |
|                                                      |
|  Envoie le paquet sign�  ------------->              |
|                                                      |
|                                                      |
|                 <------------------ V�rifie la signature du paquet
|                                                      |
|                 <------------------ V�rifie le HMAC du contenu
|                                                      |
|                 <------------------ V�rifie le nonce (anti-rejeu)
|                                                      |
|                 <------------------ V�rifie le jeton (authentification)
|                                                      |
|                                                      |
|   Traitement selon la r�ponse du serveur             |
|                                                      |

'''